namespace ifm.IoTCore.MessageHandler.Contracts;

using System.ComponentModel;
using Message;

/// <summary>
/// Provides data for an event message event.
/// </summary>
public class EventMessageEventArgs : CancelEventArgs
{
    /// <summary>
    /// The event message.
    /// </summary>
    public Message Message { get; }

    /// <summary>
    /// Creates a new instance of the class.
    /// </summary>
    /// <param name="message">The event message.</param>
    public EventMessageEventArgs(Message message)
    {
        Message = message;
    }
}