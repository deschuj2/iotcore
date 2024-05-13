namespace ifm.IoTCore.PersistenceManager;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Common;
using Common.Variant;
using Contracts;
using ElementManager.Contracts;
using ElementManager.Contracts.Elements;
using Logger.Contracts;

public class PersistenceManager : IPersistenceManager
{
    private const string Subscribe = "subscribe";
    private const string Unsubscribe = "unsubscribe";
    private const string Mirror = "mirror";
    private const string Unmirror = "unmirror";
    private const string AddElement = "addelement";
    private const string RemoveElement = "removeelement";
    private const string AddProfile = "addprofile";
    private const string RemoveProfile = "removeprofile";
    private const string AddLink = "addlink";
    private const string RemoveLink = "removelink";

    private readonly string _persistenceFileName;
    private readonly IElementCache _elementCache;
    private readonly ILogger _logger;

    private readonly LinkedList<IPersistenceManager.ServiceCallInfo> _items = new();
    private int _persistId;

    public PersistenceManager(string fileName, IElementCache elementCache, ILogger logger)
    {
        _elementCache = elementCache ?? throw new ArgumentNullException(nameof(elementCache));
        _logger = logger;

        _persistenceFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "ifm", "iotcore", $"persist_{fileName}.txt");

        ReadFromFile();
    }

    public IEnumerable<IPersistenceManager.ServiceCallInfo> PersistedItems => _items;

    public int Persist(string serviceAddress, Variant serviceData, int? cid)
    {
        lock (this)
        {
            try
            {
                // Identical command already exists
                if (_items.Any(x => x.ServiceAddress == serviceAddress && x.ServiceData.Equals(serviceData) && x.Cid == cid)) return -1;

                // Add some logic for well-known services
                switch (ElementAddress.GetLastIdentifier(serviceAddress))
                {
                    case Subscribe:
                    case Unsubscribe:
                    case Mirror:
                    case Unmirror:
                    case AddElement:
                    case RemoveElement:
                    case AddLink:
                    case RemoveLink:
                    case AddProfile:
                    case RemoveProfile:
                        break;
                }

                var item = new IPersistenceManager.ServiceCallInfo(++_persistId, serviceAddress, serviceData, cid);
                _items.AddLast(item);
                WriteToFile();
                return _persistId;
            }
            catch (Exception e)
            {
                _logger?.Error(e.Message);
            }
            return -1;
        }
    }

    public void Remove(int id)
    {
        lock (this)
        {
            var item = _items.FirstOrDefault(x => x.Id == id);
            if (item != null)
            {
                _items.Remove(item);
                WriteToFile();
            }
        }
    }

    public void Restore()
    {
        lock (this)
        {
            foreach (var item in _items)
            {
                try
                {
                    var element = _elementCache.GetElementByAddress(item.ServiceAddress);
                    if (element == null)
                    {
                        throw new Exception($"Restore persisted service {item.ServiceAddress} is not found");
                    }
                    if (element is not IServiceElement serviceElement)
                    {
                        throw new Exception($"Restore persisted service {item.ServiceAddress} is not a service");
                    }
                    serviceElement.Invoke(item.ServiceData, item.Cid);
                }
                catch (Exception e)
                {
                    _logger.Error($"Restore persisted service {item.ServiceAddress} failed: Error: {e.Message}");
                }
            }
        }
    }

    private void WriteToFile()
    {
        var items = new List<Tuple<int, string, string, int?>>();
        foreach (var item in _items)
        {
            items.Add(new Tuple<int, string, string, int?>(item.Id, 
                item.ServiceAddress, 
                VariantConverter.ToJsonElement(item.ServiceData).ToString(), 
                item.Cid));
        }
        var json = JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_persistenceFileName, json);
    }

    private void ReadFromFile()
    {
        if (File.Exists(_persistenceFileName))
        {
            try
            {
                var json = File.ReadAllText(_persistenceFileName);
                var items = JsonSerializer.Deserialize<List<Tuple<int, string, string, int?>>>(json);
                if (items == null) return;
                foreach (var item in items)
                {
                    _items.AddLast(new IPersistenceManager.ServiceCallInfo(item.Item1, 
                        item.Item2, 
                        VariantConverter.FromJsonElement(JsonDocument.Parse(item.Item3).RootElement, false), 
                        item.Item4));
                }
            }
            catch (Exception e)
            {
                _logger?.Error(e.Message);
            }
        }
    }
}