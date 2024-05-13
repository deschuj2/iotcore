namespace ifm.IoTCore;

using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Exceptions;
using Common.Variant;
using ElementManager.Contracts;
using ElementManager.Contracts.Elements;
using EventSender.Contracts;
using Logger.Contracts;
using MessageHandler.Contracts;
using NetAdapterManager.Contracts.Client;
using NetAdapterManager.Contracts.Server;
using PersistenceManager.Contracts;
using ServiceData.Requests;
using ServiceData.Responses;
using UserManager.Contracts;

public class IoTCore : IIoTCore
{
    public string Identifier { get; }
    public string Uid { get; }
    public string ApiVersion { get; }

    public IEnumerable<CatalogInfo> Catalogs => _catalogs;
    private List<CatalogInfo> _catalogs;

    public IEnumerable<ComponentInfo> Components => _components;
    private List<ComponentInfo> _components;

    public IEnumerable<string> DeviceClass => _deviceClass;
    private IEnumerable<string> _deviceClass = null;

    public IBaseElement Root { get; }
    public IUserManager UserManager { get; }
    public IElementManager ElementManager { get; }
    public IMessageHandler MessageHandler { get; }
    public IClientNetAdapterManager ClientNetAdapterManager { get; }
    public IServerNetAdapterManager ServerNetAdapterManager { get; }
    public IEventSender EventSender { get; }
    public IPersistenceManager PersistenceManager { get; }
    public ILogger Logger { get; }

    public IoTCore(string identifier,
        IUserManager userManager,
        IElementManager elementManager,
        IMessageHandler messageHandler,
        IClientNetAdapterManager clientNetAdapterManager,
        IServerNetAdapterManager serverNetAdapterManager,
        IEventSender eventSender,
        IPersistenceManager persistenceManager,
        ILogger logger)
    {
        if (string.IsNullOrEmpty(identifier)) throw new ArgumentNullException(nameof(identifier));
        if (!ElementAddress.ValidateIdentifier(identifier)) throw new ArgumentException(nameof(identifier));

        Identifier = identifier;
        Uid = null;
        ApiVersion = "V2.0";
        UserManager = userManager;
        ElementManager = elementManager;
        MessageHandler = messageHandler;
        ClientNetAdapterManager = clientNetAdapterManager;
        ServerNetAdapterManager = serverNetAdapterManager;
        EventSender = eventSender;
        PersistenceManager = persistenceManager;
        Logger = logger;

        Root = ElementManager.CreateElement(null, Identifiers.Device, identifier);
        ElementManager.Root = Root;

        ElementManager.CreateGetterServiceElement(Root, Identifiers.GetIdentity, (_, _) => GetIdentity());
        ElementManager.CreateServiceElement<GetTreeRequestServiceData, GetTreeResponseServiceData>(Root, Identifiers.GetTree, (_, d, _) => GetTree(d));
        ElementManager.CreateServiceElement<QueryTreeRequestServiceData, QueryTreeResponseServiceData>(Root, Identifiers.QueryTree, (_, d, _) => QueryTree(d));
        ElementManager.CreateServiceElement<GetDataMultiRequestServiceData, GetDataMultiResponseServiceData>(Root, Identifiers.GetDataMulti, (_, d, _) => GetDataMulti(d));
        ElementManager.CreateServiceElement<SetDataMultiRequestServiceData, SetDataMultiResponseServiceData>(Root, Identifiers.SetDataMulti, (_, d, _) => SetDataMulti(d));
        ElementManager.CreateServiceElement<GetSubscriberListRequestServiceData, GetSubscriberListResponseServiceData>(Root, Identifiers.GetSubscriberList, (_, d, _) => GetSubscriberList(d));

        var treeChangedEventElement = ElementManager.CreateEventElement(ElementManager.Root, Identifiers.TreeChanged);
        ElementManager.Root.TreeChanged += (_, _) => treeChangedEventElement.Raise();

        Logger?.Info(string.Format($"IoTCore {ApiVersion} started"));
    }

    public void AddCatalog(CatalogInfo catalogInfo)
    {
        _catalogs ??= new List<CatalogInfo>();
        _catalogs.Add(catalogInfo);
    }

    public void AddComponent(ComponentInfo componentInfo)
    {
        _components ??= new List<ComponentInfo>();
        _components.Add(componentInfo);
    }

    //private void SetDeviceClass()
    //{
    //    // ToDo: The device class is defined as the profiles from the root element.
    //}

    private GetIdentityResponseServiceData GetIdentity()
    {
        var servers = ServerNetAdapterManager.ServerNetAdapters.Select(x => new ServerInfo(x.Scheme, x.Uri.ToString(), new List<string> { x.Format })).ToList();
        var ioTInfo = new GetIdentityResponseServiceData.IoTInfo(Identifier, Uid, ApiVersion, Catalogs, Components, servers, null);

        var mode = ServerNetAdapterManager.ServerNetAdapters.Any(x => string.Equals(x.Scheme, "https", StringComparison.InvariantCultureIgnoreCase) && x.IsListening)
            ? "enabled"
            : "disabled";

        var isPasswdSet = this.UserManager.IsAuthenticationRequired
            ? "TRUE"
            : "FALSE";

        var securityInfo = new GetIdentityResponseServiceData.SecurityInfo(mode, "standard", isPasswdSet, "tcp_if");
        return new GetIdentityResponseServiceData(ioTInfo, securityInfo);
    }

    private GetTreeResponseServiceData GetTree(GetTreeRequestServiceData data)
    {
        var element = ElementManager.Root;
        var expandConstants = false;
        var expandLinks = false;
        var level = int.MaxValue;

        if (data != null)
        {
            if (string.IsNullOrEmpty(data.Address))
            {
                element = ElementManager.Root;
            }
            else
            {
                element = ElementManager.GetElementByAddress(data.Address);
                if (element == null) throw new DataInvalidException($"Element {data.Address} not found");
            }

            expandConstants = data.ExpandConstValues;
            expandLinks = data.ExpandLinks;
            if (data.Level != null)
            {
                if (data.Level < -1) throw new DataInvalidException($"Invalid level {data.Level.Value}");
                if (data.Level > -1) level = data.Level.Value;
            }
        }

        return new GetTreeResponseServiceData(element, level, expandConstants, expandLinks);
    }

    private QueryTreeResponseServiceData QueryTree(QueryTreeRequestServiceData data)
    {
        var profilePredicate = data?.Profile == null ? _ => true : new Predicate<IBaseElement>(x => x.HasProfile(data.Profile));
        var typePredicate = data?.Type == null ? _ => true : new Predicate<IBaseElement>(x => x.Type.Equals(data.Type, StringComparison.OrdinalIgnoreCase));
        var identifierPredicate = data?.Identifier == null ? _ => true : new Predicate<IBaseElement>(x => x.Identifier.Equals(data.Identifier, StringComparison.OrdinalIgnoreCase));

        var result = Root.GetElementsByPredicate(x => profilePredicate(x) && typePredicate(x) && identifierPredicate(x) && !x.IsHidden);
        return new QueryTreeResponseServiceData(result.Select(x => x.Address));
    }

    private GetDataMultiResponseServiceData GetDataMulti(GetDataMultiRequestServiceData data)
    {
        if (data == null) throw new DataInvalidException("Service data empty");
        if (data.DataToSend == null) throw new DataInvalidException("Parameter 'datatosend' missing");
        if (data.DataToSend.Count == 0) throw new DataInvalidException("Parameter 'datatosend' empty");
        if (data.DataToSend.HasDuplicates()) throw new DataInvalidException("Parameter 'datatosend' has duplicates");

        var response = new GetDataMultiResponseServiceData();
        foreach (var address in data.DataToSend)
        {
            var element = ElementManager.GetElementByAddress(address);
            if (element is IReadDataElement dataElement)
            {
                try
                {
                    response[address] = new CodeDataPair(ResponseCodes.Success, dataElement.GetValue());
                }
                catch (IoTCoreException ex)
                {
                    response[address] = new CodeDataPair(ex.ResponseCode, Variant.FromObject(new ErrorInfoResponseServiceData(ex.Message, ex.ErrorCode, ex.ErrorDetails)));
                }
                catch (Exception ex)
                {
                    response[address] = new CodeDataPair(ResponseCodes.InternalError, Variant.FromObject(new ErrorInfoResponseServiceData(ex.Message)));
                }
            }
            else
            {
                response[address] = new CodeDataPair(ResponseCodes.NotFound, null);
            }
        }
        return response;
    }

    private SetDataMultiResponseServiceData SetDataMulti(SetDataMultiRequestServiceData data)
    {
        if (data == null) throw new DataInvalidException("Service data empty");
        if (data.DataToSend == null) throw new DataInvalidException("Element 'datatosend' missing");

        var response = new SetDataMultiResponseServiceData();
        foreach (var item in data.DataToSend)
        {
            var element = ElementManager.GetElementByAddress(item.Key);
            if (element is IWriteDataElement dataElement)
            {
                try
                {
                    dataElement.SetValue(item.Value);
                    response[item.Key] = new CodeDataPair(ResponseCodes.Success, null);
                }
                catch (IoTCoreException ex)
                {
                    response[item.Key] = new CodeDataPair(ex.ResponseCode, Variant.FromObject(new ErrorInfoResponseServiceData(ex.Message, ex.ErrorCode, ex.ErrorDetails)));
                }
                catch (Exception ex)
                {
                    response[item.Key] = new CodeDataPair(ResponseCodes.InternalError, Variant.FromObject(new ErrorInfoResponseServiceData(ex.Message)));
                }
            }
            else
            {
                response[item.Key] = new CodeDataPair(ResponseCodes.NotFound, null);
            }
        }
        return response;
    }

    private GetSubscriberListResponseServiceData GetSubscriberList(GetSubscriberListRequestServiceData data)
    {
        var response = new GetSubscriberListResponseServiceData();
        if (string.IsNullOrEmpty(data?.Address))
        {
            var elements = Root.GetElementsByPredicate(x => x.Type == Identifiers.Event);
            foreach (var element in elements)
            {
                var eventElement = (IEventElement)element;
                foreach (var subscription in eventElement.GetSubscriptions())
                {
                    response.Add(eventElement.Address, subscription.Callback, subscription.DataToSend, subscription.Persist, subscription.Id);
                }
            }
        }
        else
        {
            var element = ElementManager.GetElementByAddress(data.Address);
            if (element == null)
            {
                throw new DataInvalidException($"Element {data.Address} not found");
            }
            if (element is IEventElement eventElement)
            {
                foreach (var subscription in eventElement.GetSubscriptions())
                {
                    response.Add(eventElement.Address, subscription.Callback, subscription.DataToSend, subscription.Persist, subscription.Id);
                }
            }
            else
            {
                throw new DataInvalidException($"Element {data.Address} is not an event element");
            }
        }

        return response;
    }

    public void Dispose()
    {
        EventSender?.Dispose();
        ServerNetAdapterManager?.Dispose();
        ClientNetAdapterManager?.Dispose();
    }
}