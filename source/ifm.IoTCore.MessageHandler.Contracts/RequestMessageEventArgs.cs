namespace ifm.IoTCore.MessageHandler.Contracts;

using System.ComponentModel;
using Message;

/// <summary>
/// Provides data for a request message event.
/// </summary>
public class RequestMessageEventArgs : CancelEventArgs
{
    /// <summary>
    /// The request message.
    /// </summary>
    public Message RequestMessage { get; }

    /// <summary>
    /// The response message.
    /// </summary>
    public Message ResponseMessage { get; set; }

    /// <summary>
    /// Creates a new instance of the class.
    /// </summary>
    /// <param name="requestMessage">The request message.</param>
    /// <param name="responseMessage">The response message.</param>
    public RequestMessageEventArgs(Message requestMessage, Message responseMessage = null)
    {
        RequestMessage = requestMessage;
        ResponseMessage = responseMessage;
    }
}
