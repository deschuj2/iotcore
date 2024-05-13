namespace ifm.IoTCore.ElementManager.Contracts.Elements;

using System;
using System.Collections.Generic;
using ServiceData.Requests;
using ServiceData.Responses;

/// <summary>
/// Represents a subscription.
/// </summary>
public class SubscriptionInfo : EventArgs
{
    /// <summary>
    /// The id which identifies the subscription.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The callback address of the subscription.
    /// </summary>
    public string Callback { get; }

    /// <summary>
    /// List of data element addresses, whose values are sent with the event.
    /// </summary>
    public List<string> DataToSend { get; }

    /// <summary>
    /// If true the subscription is persistent; otherwise not.
    /// </summary>
    public bool Persist { get; }

    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="id">The id which identifies the subscription.</param>
    /// <param name="callback">The callback address of the subscription.</param>
    /// <param name="dataToSend">List of data element addresses, whose values are sent with the event.</param>
    /// <param name="persist">If true the subscription is persistent; otherwise not.</param>
    public SubscriptionInfo(int id, string callback, List<string> dataToSend, bool persist)
    {
        Id = id;
        Callback = callback;
        DataToSend = dataToSend;
        Persist = persist;
    }
}

/// <summary>
/// Provides functionality to interact with an event element.
/// </summary>
public interface IEventElement : IBaseElement
{
    /// <summary>
    /// Adds a subscription to the element.
    /// </summary>
    /// <param name="data">The subscription to add.</param>
    /// <param name="cid">The context id for the service call.</param>
    SubscribeResponseServiceData Subscribe(SubscribeRequestServiceData data, int? cid = null);

    /// <summary>
    /// Removes a subscription from the element.
    /// </summary>
    /// <param name="data">The subscription to remove.</param>
    /// <param name="cid">The context id for the service call.</param>
    void Unsubscribe(UnsubscribeRequestServiceData data, int? cid = null);

    /// <summary>
    /// The subscribed event.
    /// </summary>
    event EventHandler<SubscriptionInfo> Subscribed;

    /// <summary>
    /// The unsubscribed event.
    /// </summary>
    event EventHandler<SubscriptionInfo> Unsubscribed;

    /// <summary>
    /// The event which is raised.
    /// </summary>
    event EventHandler EventRaised;

    /// <summary>
    /// Raises the event.
    /// </summary>
    void Raise();

    /// <summary>
    /// Gets the subscriptions.
    /// </summary>
    /// <returns>The subscriptions.</returns>
    List<SubscriptionInfo> GetSubscriptions();

    /// <summary>
    /// Gets the number of subscriptions.
    /// </summary>
    int SubscriptionCount { get; }
}
