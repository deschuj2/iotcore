namespace ifm.IoTCore.ElementManager;

using System.Collections.Generic;
using Contracts.Elements;
using EventSender.Contracts;

// ToDo: Move to separate assemblies SubscriptionManager.Contracts
public interface ISubscriptionManager
{
    void AddSubscription(IEventElement element, SubscriptionInfo subscription);
    void RemoveSubscription(IEventElement element, SubscriptionInfo subscription);
    List<SubscriptionInfo> GetSubscriptions(IEventElement element);
    void OnSubscribedRaised(IEventElement element, SubscriptionInfo subscription);
    void OnUnsubscribedRaised(IEventElement element, SubscriptionInfo subscription);
    void OnEventRaised(IEventElement element);
}

// ToDo: Move to separate assemblies SubscriptionManager
// ToDo: In element manager wire events to subsciption manager
// ToDo: Move eventdata from sender to here. Sender just sends
public class SubscriptionManager : ISubscriptionManager
{
    private readonly Dictionary<IEventElement, Dictionary<int, SubscriptionInfo>> _subscriptions = new();
    private readonly IEventSender _eventSender;

    public SubscriptionManager(IEventSender eventSender)
    {
        _eventSender = eventSender;
    }

    public void SendEvents(IEventElement element, List<SubscriptionInfo> subscriptions)
    {
        _eventSender.SendEvent(element, subscriptions);
    }

    public void AddSubscription(IEventElement element, SubscriptionInfo subscription)
    {
        throw new System.NotImplementedException();
    }

    public void RemoveSubscription(IEventElement element, SubscriptionInfo subscription)
    {
        throw new System.NotImplementedException();
    }

    public List<SubscriptionInfo> GetSubscriptions(IEventElement element)
    {
        throw new System.NotImplementedException();
    }

    public void OnSubscribedRaised(IEventElement element, SubscriptionInfo subscription)
    {
        throw new System.NotImplementedException();
    }

    public void OnUnsubscribedRaised(IEventElement element, SubscriptionInfo subscription)
    {
        throw new System.NotImplementedException();
    }

    public void OnEventRaised(IEventElement element)
    {
        throw new System.NotImplementedException();
    }
}