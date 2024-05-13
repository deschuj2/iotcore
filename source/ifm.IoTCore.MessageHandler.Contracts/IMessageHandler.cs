namespace ifm.IoTCore.MessageHandler.Contracts;

using System;
using Common.Variant;
using Message;

/// <summary>
/// Provides functionality to interact with the message handler.
/// </summary>
public interface IMessageHandler
{
    /// <summary>
    /// The event that is raised before the incoming request is processed.
    /// </summary>
    event EventHandler<RequestMessageEventArgs> RequestMessageReceived;

    /// <summary>
    /// The event that is raised, before the incoming event is processed.
    /// </summary>
    event EventHandler<EventMessageEventArgs> EventMessageReceived;

    /// <summary>
    /// Handles a request.
    /// </summary>
    /// <param name="message">The request message.</param>
    /// <returns>The response message.</returns>
    Message HandleRequest(Message message);

    /// <summary>
    /// Handles a request.
    /// </summary>
    /// <param name="cid">The cid for the request.</param>
    /// <param name="address">The target service address for the request.</param>
    /// <param name="data">The data for the request.</param>
    /// <param name="reply">The reply address for the request's response.</param>
    /// <returns>The response message</returns>
    Message HandleRequest(int cid, string address, Variant data = null, string reply = null);


    /// <summary>
    /// Handles an event.
    /// </summary>
    /// <param name="message">The event message.</param>
    void HandleEvent(Message message);
}