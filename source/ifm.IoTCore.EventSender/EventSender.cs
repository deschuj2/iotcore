namespace ifm.IoTCore.EventSender;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Common;
using Common.Exceptions;
using Common.Variant;
using Contracts;
using ElementManager.Contracts;
using ElementManager.Contracts.Elements;
using Logger.Contracts;
using Message;
using NetAdapterManager.Contracts.Client;
using NetAdapterManager.Contracts.Server;

public class EventSender : DisposableBase, IEventSender
{
    private readonly IElementCache _elementCache;
    private readonly IClientNetAdapterManager _clientNetAdapterManager;
    private readonly IServerNetAdapterManager _serverNetAdapterManager;
    private readonly ILogger _logger;

    private readonly BlockingCollection<Tuple<IEventElement, List<SubscriptionInfo>>> _eventQueue = new();
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private readonly CancellationToken _cancellationToken;
    private int _isStarted;

    private Task _sendTask;
    private int _eventNumber;

    public EventSender(IElementCache elementCache, IClientNetAdapterManager clientNetAdapterManager, IServerNetAdapterManager serverNetAdapterManager, ILogger logger)
    {
        _elementCache = elementCache;
        _clientNetAdapterManager = clientNetAdapterManager;
        _serverNetAdapterManager = serverNetAdapterManager;
        _logger = logger;
        _cancellationToken = _cancellationTokenSource.Token;

        Start();
    }

    public void SendEvent(IEventElement element, List<SubscriptionInfo> subscriptions)
    {
        _eventQueue.Add(new Tuple<IEventElement, List<SubscriptionInfo>>(element, subscriptions), CancellationToken.None);
    }

    private void Start()
    {
        try
        {
            _sendTask = Task.Run(SendEvents, _cancellationToken);
            Interlocked.Increment(ref _isStarted);
        }
        catch (TaskCanceledException)
        {
            // Normal on cancellation. Ignore
        }
    }

    private void SendEvents()
    {
        while (!_cancellationToken.IsCancellationRequested)
        {
            try
            {
                _eventQueue.TryTake(out var eventInfo, -1, _cancellationToken);

                foreach (var subscription in eventInfo.Item2)
                {
                    try
                    {
                        var eventServiceData = CreateEventServiceData(eventInfo.Item1, subscription);

                        if (!TrySendFromServerToConnectedClient(subscription, eventServiceData))
                        {
                            if (!TrySendFromClientToServer(subscription, eventServiceData))
                            {
                                if (!TryInvokeService(subscription, eventServiceData))
                                {
                                    _logger?.Error($"Send event to {subscription.Callback} failed: No suitable method to dispatch event available");
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        _logger?.Error($"Send event to {subscription.Callback} failed: {e.Message}");
                    }
                }

            }
            catch (OperationCanceledException)
            {
                // Normal on stop, ignore
            }
            catch (Exception e)
            {
                _logger?.Error(e.Message);
            }
        }
    }

    private bool TrySendFromServerToConnectedClient(SubscriptionInfo subscription, EventServiceData eventServiceData)
    {
        // If authority is provided event is not for a connected client
        if (subscription.Callback.IndexOf("://", StringComparison.Ordinal) != -1)
        {
            return false;
        }

        var scheme = subscription.Callback.Left(':');
        if (scheme == null)
        {
            return false;
        }

        var target = subscription.Callback.Right(':');
        if (target == null)
        {
            return false;
        }

        var path = target.Left('?');

        var query = target.Right('?');
        if (query == null)
        {
            return false;
        }

        var clientId = HttpUtility.ParseQueryString(query).Get("clientid");
        if (clientId == null)
        {
            return false;
        }

        var servers = _serverNetAdapterManager.FindServerNetAdapters(scheme);
        foreach (var server in servers)
        {
            if (server is IConnectedServerNetAdapter connectedServer)
            {
                if (connectedServer.IsClientConnected(clientId))
                {
                    var eventMessage = new Message(RequestCodes.Event,
                        0,
                        path,
                        Variant.FromObject(eventServiceData));

                    connectedServer.SendEvent(clientId, eventMessage);

                    return true;
                }
            }
        }

        return false;
    }

    private bool TrySendFromClientToServer(SubscriptionInfo subscription, EventServiceData eventServiceData)
    {
        if (Uri.TryCreate(subscription.Callback, UriKind.Absolute, out var uri))
        {
            var eventMessage = new Message(RequestCodes.Event,
                eventServiceData.SubscriptionId ?? 0,
                uri.LocalPath,
                Variant.FromObject(eventServiceData));

            var client = _clientNetAdapterManager.CreateClientNetAdapter(new Uri(subscription.Callback));
            client.SendEvent(eventMessage);

            return true;
        }

        return false;
    }

    private bool TryInvokeService(SubscriptionInfo subscription, EventServiceData eventServiceData)
    {
        var element = _elementCache.GetElementByAddress(subscription.Callback);
        if (element is IServiceElement serviceElement)
        {
            serviceElement.Invoke(Variant.FromObject(eventServiceData));

            return true;
        }

        return false;
    }

    private EventServiceData CreateEventServiceData(IEventElement eventElement, SubscriptionInfo subscription)
    {
        Dictionary<string, CodeDataPair> payload = null;
        if (subscription.DataToSend != null && subscription.DataToSend.Count > 0)
        {
            payload = new Dictionary<string, CodeDataPair>();
            foreach (var address in subscription.DataToSend)
            {
                var element = _elementCache.GetElementByAddress(address);
                if (element is IReadDataElement dataElement)
                {
                    try
                    {
                        payload[address] = new CodeDataPair(ResponseCodes.Success, dataElement.GetValue());
                    }
                    catch (IoTCoreException ex)
                    {
                        payload[address] = new CodeDataPair(ex.ResponseCode, Variant.FromObject(new ErrorInfoResponseServiceData(ex.Message, ex.ErrorCode, ex.ErrorDetails)));
                    }
                    catch
                    {
                        payload[address] = new CodeDataPair(ResponseCodes.InternalError, null);
                    }
                }
                else
                {
                    payload[address] = new CodeDataPair(ResponseCodes.NotFound, null);
                }
            }
        }
        if (eventElement.Identifier == Identifiers.DataChanged && eventElement.Parent is IReadDataElement parentElement)
        {
            payload ??= new Dictionary<string, CodeDataPair>();
            try
            {
                payload[parentElement.Address] = new CodeDataPair(ResponseCodes.Success, parentElement.GetValue());
            }
            catch (IoTCoreException ex)
            {
                payload[parentElement.Address] = new CodeDataPair(ex.ResponseCode, Variant.FromObject(new ErrorInfoResponseServiceData(ex.Message, ex.ErrorCode, ex.ErrorDetails)));
            }
            catch
            {
                payload[parentElement.Address] = new CodeDataPair(ResponseCodes.InternalError, null);
            }
        }

        return new EventServiceData(++_eventNumber, eventElement.Address, payload, subscription.Id);
    }

    protected override void Dispose(bool disposing)
    {
        if (!disposing) return;
        if (Interlocked.CompareExchange(ref _isStarted, 0, 1) != 1) return;
        _cancellationTokenSource.Cancel();

        try
        {
            _sendTask.Wait(CancellationToken.None);
        }
        catch (OperationCanceledException)
        {
            // Ignore
        }
        catch (AggregateException)
        {
            // Ignore
        }
        catch (ObjectDisposedException)
        {
            // Ignore
        }
    }
}