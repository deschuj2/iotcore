namespace ifm.IoTCore.NetAdapter.Http;

using System;
using Common;
using Common.Variant;
using Contracts;
using Logger.Contracts;
using Message;
using MessageConverter.Contracts;
using MessageHandler.Contracts;
using RequestMessageEventArgs = NetAdapterManager.Contracts.RequestMessageEventArgs;

public class HttpServerNetAdapter : IHttpServerNetAdapter
{
    private readonly IMessageHandler _messageHandler;
    private readonly HttpServer _server;

    public string Scheme => "http";

    public string Format => _server.Converter.Type;

    public Uri Uri => _server.Uri;

    public bool IsListening => _server.IsListening;

    public event EventHandler<RequestMessageEventArgs>? RequestReceived;
    public event EventHandler<RequestMessageEventArgs>? RequestProcessed;
    public event EventHandler<IoTCore.NetAdapterManager.Contracts.EventMessageEventArgs>? EventReceived;

    private object _lock = new();

    public HttpServerNetAdapter(IMessageHandler messageHandler, Uri uri, IMessageConverter converter, ILogger? logger = null)
    {
        _messageHandler = messageHandler ?? throw new ArgumentNullException(nameof(messageHandler));

        _server = new HttpServer(uri, converter, logger);

        _server.RequestReceived += OnServerOnRequestReceivedAll;
        _server.EventReceived += (sender, args) => HandleEventReceived(args);
    }

    private void OnServerOnRequestReceivedAll(object sender, RequestMessageEventArgs args)
    {
        HandleRequestReceived(args);
    }

    private void OnServerOnRequestReceivedGetIdentityOnly(object sender, RequestMessageEventArgs args)
    {
        HandleGetIdentityRequestReceived(args);
    }

    private void HandleRequestReceived(RequestMessageEventArgs args)
    {
        RequestReceived.Raise(this, args);
        if (!args.Cancel)
        {
            args.ResponseMessage = _messageHandler.HandleRequest(args.RequestMessage);
        }
        RequestProcessed?.Raise(this, args);
    }

    private void HandleGetIdentityRequestReceived(RequestMessageEventArgs args)
    {
        var address = ElementAddress.GetParentAddress(args.RequestMessage.Address);
        if (address.Equals($"/{Identifiers.GetIdentity}", StringComparison.OrdinalIgnoreCase))
        {
            args.ResponseMessage = _messageHandler.HandleRequest(args.RequestMessage);
        }
        else
        {
            args.ResponseMessage = new Message(ResponseCodes.BadRequest, 
                args.RequestMessage.Cid, 
                args.RequestMessage.Address,
                Variant.FromObject(new ErrorInfoResponseServiceData("Only getidentity is supported by this server")));
        }
    }

    private void HandleEventReceived(IoTCore.NetAdapterManager.Contracts.EventMessageEventArgs args)
    {
        EventReceived?.Raise(this, args);
        if (!args.Cancel)
        {
            _messageHandler.HandleEvent(args.Message);
        }
    }

    public void Start() => _server.Start();

    public void Stop() => _server.Stop();

    public void Dispose() => _server.Dispose();

    private AllowedServicesMode _allowedServicesMode = AllowedServicesMode.All;

    public AllowedServicesMode AllowedServicesMode
    {
        get
        {
            lock (_lock)
            {
                return _allowedServicesMode;
            }
            
        }
        set
        {
            lock (_lock)
            {
                if (_allowedServicesMode != value)
                {
                    _allowedServicesMode = value;
                    
                    if (value == AllowedServicesMode.All)
                    {
                        _server.RequestReceived -= OnServerOnRequestReceivedGetIdentityOnly;
                        _server.RequestReceived += OnServerOnRequestReceivedAll;
                    }
                    else if (value == AllowedServicesMode.GetIdentityOnly)
                    {
                        _server.RequestReceived -= OnServerOnRequestReceivedAll;
                        _server.RequestReceived += OnServerOnRequestReceivedGetIdentityOnly;
                    }
                    else
                    {
                        throw new InvalidOperationException("Unknown mode switch");
                    }
                }
            }
        }
    }
}