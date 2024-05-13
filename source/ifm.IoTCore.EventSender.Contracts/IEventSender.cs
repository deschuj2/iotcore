namespace ifm.IoTCore.EventSender.Contracts;

using System;
using System.Collections.Generic;
using ElementManager.Contracts.Elements;

/// <summary>
/// Provides functionality to interact with the event sender.
/// </summary>
public interface IEventSender : IDisposable
{
    /// <summary>
    /// Sends and event on behalf of the given event element to the recipient in the subscription.
    /// </summary>
    /// <param name="element">The event element which raised an event.</param>
    /// <param name="subscriptions">The subscriptions of the event element.</param>
    void SendEvent(IEventElement element, List<SubscriptionInfo> subscriptions);
}