namespace ifm.IoTCore.NetAdapter.Http;

using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using Common;
using Common.Exceptions;
using Message;
using MessageConverter.Contracts;
using NetAdapterManager.Contracts.Client;

public class HttpClientNetAdapter : HttpClientNetAdapterBase
{
    public HttpClientNetAdapter(Uri remoteUri, 
        IMessageConverter converter, 
        TimeSpan timeout, 
        bool keepAlive) : base(remoteUri, converter, timeout, keepAlive)
    {
    }
}


public class HttpClientNetAdapterBase : IClientNetAdapter
{
        
    private readonly IMessageConverter _converter;

    protected HttpClient Client { get; set; }

    public HttpClientNetAdapterBase(Uri remoteUri, IMessageConverter converter, TimeSpan timeout, bool keepAlive)
    {
        RemoteUri = remoteUri;
        _converter = converter;

        Client = new HttpClient
        {
            Timeout = timeout,
        };

        if (!keepAlive)
        {
            return;
        }

        Client.DefaultRequestHeaders.ConnectionClose = false;
        Client.DefaultRequestHeaders.Add("Connection", "keep-alive");
    }

    public DateTime LastUsed { get; private set; }

    public Uri RemoteUri { get; }

    public Message SendRequest(Message requestMessage)
    {
        LastUsed = DateTime.Now;
        var requestString = _converter.Serialize(requestMessage);

        string? responseString;

        var cts = new CancellationTokenSource();
        cts.CancelAfter(Client.Timeout);

        try
        {
            var httpResponse = Client.PostAsync(RemoteUri, CreateStringContent(requestString, _converter.ContentType), cts.Token).GetAwaiter().GetResult();
            responseString = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        }
        catch (HttpRequestException e)
        {
            // We always get a timeout after 21 seconds even if we set the timeout higher!
            // The 21 seconds is the built -in default for TCP connection timeout on Windows.
            // It is 21 seconds because Windows TCP stack allows for 7 seconds per leg of the 3 - leg TCP SYNC - ACK connection setup.
            // There is no current override to this timeout which is a built-in timeout to the Windows TCP layer.
            // https://github.com/dotnet/runtime/issues/27232
            // https://getin2me.blogspot.com/2010/08/are-you-also-facing-windows-sockets-21.html

            // What can we do now? We get a timeout after round about 21sec. Because of tcp timeout (Windows)!
            // But if we set a timeout > 21sec, our api should return a timeout exception in that time (> 21sec)
            // Workaround:
            // This HttpRequestException has an internal SocketException and a Native/ErrorCode 10060.
            // WSAETIMEDOUT: 1006 --> Socket Connection Timeout Error
            if (e.InnerException is SocketException { NativeErrorCode: 10060 } && !cts.IsCancellationRequested)
            {
                // The token timeout is also set to the user timeout. So we WAIT until
                // this timeout has reached and throw our own timeout exception.
                WaitHandle.WaitAny(new[] { cts.Token.WaitHandle });
                if (cts.Token.IsCancellationRequested)
                {
                    throw new IoTCoreException(ResponseCodes.Timeout, e.Message);
                }
            }

            var message = e.InnerException?.Message != null
                ? $"{e.Message} InnerException message: '{e.InnerException.Message}'"
                : e.Message;

            throw new IoTCoreException(ResponseCodes.Timeout, message);
        }
        catch (Exception e)
        {
            // The task of Client.PostAsync() does not return a TimeoutException.
            // A TaskCancellationException is always thrown with the exception text that a timeout has occurred.
            // A separate cts is the workaround for this.
            // Another workaround can be: https://thomaslevesque.com/2018/02/25/better-timeout-handling-with-httpclient/

            if (cts.Token.IsCancellationRequested)
            {
                throw new IoTCoreException(ResponseCodes.Timeout, e.Message);
            }

            throw new IoTCoreException(ResponseCodes.InternalError, e.Message);
        }

        return _converter.Deserialize(responseString);
    }

    public void SendEvent(Message eventMessage)
    {
        LastUsed = DateTime.Now;
        var eventString = _converter.Serialize(eventMessage);

        var cts = new CancellationTokenSource();
        cts.CancelAfter(Client.Timeout);

        try
        {
            Client.PostAsync(RemoteUri, CreateStringContent(eventString, _converter.ContentType), cts.Token).GetAwaiter().GetResult();
        }
        catch (HttpRequestException e)
        {
            var message = e.InnerException?.Message != null
                ? $"{e.Message} InnerException message: '{e.InnerException.Message}'"
                : e.Message;

            throw new IoTCoreException(ResponseCodes.Timeout, message);
        }
        catch (Exception e)
        {
            // The task of Client.PostAsync() does not return a TimeoutException.
            // A TaskCancellationException is always thrown with the exception text that a timeout has occurred.
            // A separate cts is the workaround for this.
            // Another workaround can be: https://thomaslevesque.com/2018/02/25/better-timeout-handling-with-httpclient/

            if (cts.Token.IsCancellationRequested)
            {
                throw new IoTCoreException(ResponseCodes.Timeout, e.Message);
            }

            throw new IoTCoreException(ResponseCodes.InternalError, e.Message);
        }
    }

    /// <summary>
    /// Rewrites the Content-Type Header.
    /// </summary>
    /// <param name="content">The content of the <seealso cref="StringContent"/>.</param>
    /// <param name="contentType">The content-type of the <seealso cref="StringContent"/>.</param>
    /// <returns>The created <seealso cref="StringContent"/> instance.</returns>
    /// <remarks>The rewrite is necessary, since the content-type header "application/json" will be initialized to "application/json; charset=utf-8" by the <seealso cref="StringContent"/> class.</remarks>
    /// <remarks>That is unexpected because it is also mentioned in the https://www.rfc-editor.org/rfc/rfc8259#page-11 that there are no required nor optional parameters.</remarks>
    /// <remarks>The Content-Type of "text/html" in opposition defines a charset parameter. See https://www.iana.org/assignments/media-types/text/html</remarks>
    private StringContent CreateStringContent(string content, string contentType)
    {
        var stringContent = new StringContent(content, Encoding.UTF8, contentType);

        if (contentType.ToLowerInvariant() != "application/json")
        {
            return stringContent;
        }

        stringContent.Headers.Remove("Content-Type");
        stringContent.Headers.Add("Content-Type", contentType);

        return stringContent;
    }

    public void Dispose()
    {
        Client.Dispose();
    }
}