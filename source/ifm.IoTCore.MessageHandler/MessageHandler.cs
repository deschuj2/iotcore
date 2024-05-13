namespace ifm.IoTCore.MessageHandler;

using System;
using Common;
using Common.Exceptions;
using Common.Variant;
using Contracts;
using ElementManager.Contracts;
using ElementManager.Contracts.Elements;
using Logger.Contracts;
using Message;
using UserManager.Contracts;

public class MessageHandler : IMessageHandler
{
    private readonly string _rootIdentifier;
    private readonly IElementCache _elementCache;
    private readonly IUserManager _userManager;
    private readonly ILogger _logger;

    public event EventHandler<RequestMessageEventArgs> RequestMessageReceived;
    public event EventHandler<EventMessageEventArgs> EventMessageReceived;

    public MessageHandler(string rootIdentifier, IElementCache elementCache, IUserManager userManager, ILogger logger)
    {
        _rootIdentifier = rootIdentifier;
        _elementCache = elementCache;
        _userManager = userManager;
        _logger = logger;
    }

    public Message HandleRequest(int cid, string address, Variant data = null, string reply = null)
    {
        return HandleRequest(new Message(RequestCodes.Request, cid, address, data, reply));
    }

    public Message HandleRequest(Message message)
    {
        _logger?.Debug($"Request: code={message.Code}, address={message.Address}, data={message.Data}");

        Message response;
        try
        {
            response = InnerHandleRequest(message);
        }
        catch (IoTCoreException e)
        {
            response = new Message(e.ResponseCode,
                message.Cid,
                message.Reply ?? message.Address,
                Variant.FromObject(new ErrorInfoResponseServiceData(e.Message, e.ErrorCode, e.ErrorDetails)));
        }

        if (response.Code != ResponseCodes.Success)
        {
            _logger?.Error($"Service {response.Address} failed: code={response.Code} data={response.Data}");
        }
        else
        {
            _logger?.Debug($"Response: code={response.Code}, address={response.Address}, data={response.Data}");
        }

        return response;
    }

    private Message InnerHandleRequest(Message message)
    {
        if (message == null)
        {
            throw new DataInvalidException($"Argument '{nameof(message)}' null or empty");
        }

        if (RaiseRequestMessageEvent(message, out var responseMessage))
        {
            return responseMessage;
        }

        if (_userManager.IsAuthenticationRequired)
        {
            if (ElementAddress.PatchAddress(_rootIdentifier, message.Address) != ElementAddress.PatchAddress(_rootIdentifier, "/getidentity")
                && !_userManager.Authenticate(message.Authentication?.User, message.Authentication?.Password))
            {
                throw new IoTCoreException(ResponseCodes.AccessDenied, "Authentication failed");
            }
        }

        var element = _elementCache.GetElementByAddress(message.Address);
        if (element == null)
        {
            throw new IoTCoreException(ResponseCodes.NotFound, $"Element '{message.Address}' not found");
        }
        if (element is not IServiceElement serviceElement)
        {
            throw new IoTCoreException(ResponseCodes.BadRequest, $"Element '{message.Address}' is not a service");
        }

        try
        {
            var data = serviceElement.Invoke(message.Data, message.Cid);
            return new Message(ResponseCodes.Success, message.Cid, message.Reply ?? message.Address, data);
        }
        catch (IoTCoreException)
        {
            throw;
        }
        catch (Exception e)
        {
            throw new IoTCoreException(ResponseCodes.InternalError, $"Service '{serviceElement.Address}' failed", e.Message);
        }
    }

    private bool RaiseRequestMessageEvent(Message requestMessage, out Message responseMessage)
    {
        var args = new RequestMessageEventArgs(requestMessage);
        RequestMessageReceived?.Raise(this, args);
        responseMessage = args.ResponseMessage;
        return args.Cancel;
    }

    public void HandleEvent(Message message)
    {
        try
        {
            InnerHandleEvent(message);
        }
        catch (IoTCoreException e)
        {
            _logger?.Error($"Service {message.Address} failed. {e.Message} {e.ResponseCode} {e.ErrorDetails}");
        }
    }

    public void InnerHandleEvent(Message message)
    {
        if (message == null)
        {
            throw new DataInvalidException($"Argument '{nameof(message)}' null or empty");
        }

        if (RaiseEventMessageEvent(message))
        {
            return;
        }

        var element = _elementCache.GetElementByAddress(message.Address);
        if (element == null)
        {
            throw new IoTCoreException(ResponseCodes.NotFound, $"Element '{message.Address}' not found");
        }
        if (element is not IServiceElement serviceElement)
        {
            throw new IoTCoreException(ResponseCodes.BadRequest, $"Element '{message.Address}' is not a service");
        }
        try
        {
            serviceElement.Invoke(message.Data, message.Cid);
        }
        catch (IoTCoreException)
        {
            throw;
        }
        catch (Exception e)
        {
            throw new IoTCoreException(ResponseCodes.InternalError, $"Service '{serviceElement.Address}' failed", e.Message);
        }
    }

    private bool RaiseEventMessageEvent(Message message)
    {
        var args = new EventMessageEventArgs(message);
        EventMessageReceived?.Raise(this, args);
        return args.Cancel;
    }
}