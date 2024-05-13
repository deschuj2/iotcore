namespace ifm.IoTCore.ElementManager.Elements;

using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Exceptions;
using Common.Variant;
using Contracts.Elements;
using Contracts.Elements.Formats;
using Contracts.Elements.ServiceData.Requests;
using Contracts.Elements.ServiceData.Responses;
using PersistenceManager.Contracts;

internal sealed class EventElement : BaseElement, IEventElement
{
    private readonly IPersistenceManager _persistenceManager;
    private EventHandler _eventRaisedHandler;
    private readonly Dictionary<int, SubscriptionInfo> _subscriptions = new();


    public EventElement(string identifier,
        string address,
        IPersistenceManager persistenceManager,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false) : base(Identifiers.Event, identifier, address, format, profiles, uid, isHidden)
    {
        _persistenceManager = persistenceManager ?? throw new ArgumentNullException(nameof(persistenceManager));
    }

    public event EventHandler<SubscriptionInfo> Subscribed;

    public event EventHandler<SubscriptionInfo> Unsubscribed;

    public event EventHandler EventRaised
    {
        add
        {
            _eventRaisedHandler += value;
            Subscribed.Raise(this);
        }

        remove
        {
            _eventRaisedHandler -= value;
            Unsubscribed.Raise(this);
        }
    }

    public List<SubscriptionInfo> GetSubscriptions()
    {
        lock (_subscriptions)
        {
            return _subscriptions.Values.ToList();
        }
    }

    public int SubscriptionCount
    {
        get
        {
            lock (_subscriptions)
            {
                return _subscriptions.Count;
            }
        }
    }

    public void Raise()
    {
        _eventRaisedHandler.Raise(this);
    }

    public SubscribeResponseServiceData SubscribeFunc(IBaseElement element, SubscribeRequestServiceData data, int? cid)
    {
        var response = Subscribe(data, cid);
        if (data.Persist) _persistenceManager?.Persist(element.Address, Variant.FromObject(data), cid);
        return response;
    }

    public void UnsubscribeFunc(IBaseElement element, UnsubscribeRequestServiceData data, int? cid)
    {
        Unsubscribe(data, cid);
        _persistenceManager?.Persist(element.Address, Variant.FromObject(data), cid);
    }

    public SubscribeResponseServiceData Subscribe(SubscribeRequestServiceData data, int? cid)
    {
        if (data == null) throw new DataInvalidException("Service data empty");
        if (!ElementAddress.IsValidUri(data.Callback)) throw new DataInvalidException($"{data.Callback} is not a valid uri");
        if (data.DataToSend != null && data.DataToSend.HasDuplicates()) throw new DataInvalidException("Parameter 'datatosend' has duplicates");

        lock (_subscriptions)
        {
            var subscriptionId = GenerateSubscriptionId(data.SubscriptionId, cid);
            var subscription = new SubscriptionInfo(subscriptionId, data.Callback, data.DataToSend, data.Persist);
            _subscriptions[subscriptionId] = subscription;
            try
            {
                Subscribed.Raise(this, subscription, true, true);
            }
            catch
            {
                _subscriptions.Remove(subscriptionId);
                throw;
            }
            return new SubscribeResponseServiceData(subscriptionId);
        }
    }

    public void Unsubscribe(UnsubscribeRequestServiceData data, int? cid)
    {
        if (data == null) throw new DataInvalidException("Service data empty");
        lock (_subscriptions)
        {
            if (data.SubscriptionId != null)
            {
                if (!_subscriptions.Remove(data.SubscriptionId.Value))
                {
                    throw new DataInvalidException("Subscription not found", $"id = {data.SubscriptionId.Value}");
                }
            }
            else
            {
                if (_subscriptions.Values.Any(x => x.Callback == data.Callback && x.Id == cid))
                {
                    _subscriptions.RemoveAll(x => x.Callback == data.Callback && x.Id == cid);
                }
                else
                {
                    throw new DataInvalidException("Subscription not found", $"callback = {data.Callback}");
                }
            }

            try
            {
                Unsubscribed.Raise(this, new SubscriptionInfo(data.SubscriptionId ?? 0, data.Callback, null, true));
            }
            catch
            {
                // Ignore and log. Unsubscribe should never fail
            }
        }
    }

    private int GenerateSubscriptionId(int? subscriptionId, int? cid)
    {
        const int autoCreateSubscriptionId = -1;

        var id = autoCreateSubscriptionId;
        if (subscriptionId != null)
        {
            id = subscriptionId.Value;
        }
        else if (cid != null)
        {
            id = cid.Value;
        }
        if (id == autoCreateSubscriptionId)
        {
            id = 0;
            while (id < short.MaxValue)
            {
                if (_subscriptions.Values.Any(x => x.Id == id))
                {
                    id++;
                }
                else
                {
                    break;
                }
            }
        }
        return id;
    }
}
