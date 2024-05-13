namespace ifm.IoTCore.NetAdapter.Http;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Exceptions;
using Common.Variant;
using Logger.Contracts;
using Message;
using MessageConverter.Contracts;

internal class HttpServer : IDisposable
{
    private readonly HttpListener _httpListener = new();
    private readonly ILogger? _logger;
    private bool _disposed;

    public readonly Uri Uri;
    public readonly IMessageConverter Converter;

    public event EventHandler<IoTCore.NetAdapterManager.Contracts.RequestMessageEventArgs>? RequestReceived;
    public event EventHandler<IoTCore.NetAdapterManager.Contracts.EventMessageEventArgs>? EventReceived;

    public bool IsListening => _httpListener.IsListening;

    public HttpServer(Uri uri, IMessageConverter converter, ILogger? logger)
    {
        Uri = uri ?? throw new ArgumentNullException(nameof(uri));
        Converter = converter ?? throw new ArgumentNullException(nameof(converter));
        _logger = logger;

        _httpListener.Prefixes.Add($"{uri.Scheme}://{(uri.Host == IPAddress.Any.ToString() ? "*" : uri.Host)}:{uri.Port}/");
    }

    public void Start()
    {
        if (_httpListener.IsListening) return;

        _httpListener.Start();
        Task.Run(Listen);
    }

    public void Stop()
    {
        if (!_httpListener.IsListening) return;

        _httpListener.Stop();

        // ToDo: Wait for all tasks to return
    }

    private void Listen()
    {
        while (_httpListener.IsListening && !_disposed)
        {
            try
            {
                var context = _httpListener.GetContext();
                Task.Run(() => HandleRequest(context));
            }
            catch (Exception e)
            {
                _logger?.Error(e.Message);
            }
        }
    }

    protected void HandleRequest(HttpListenerContext context)
    {
        try
        {
            if (context.Request.HttpMethod == "GET")
            {
                if (context.Request.Url.AbsolutePath == "/web/subscribe" || context.Request.Url.AbsolutePath == "/browse" || ( context.Request.Url.AbsolutePath == "/" && !context.Request.HasEntityBody ))
                {
                    var webPage = GetIotCoreBrowserWebPage();
                    context.Response.AppendHeader("Content-Encoding", "gzip");
                    context.Response.ContentType = "text/html";
                    context.Response.ContentLength64 = webPage.Length;
                    context.Response.OutputStream.Write(webPage, 0, webPage.Length);
                }
                else if (context.Request.Url.AbsolutePath.StartsWith("/ecologone"))
                {
                    var webPage = GetIotCoreBrowserFile("/index.html");
                    context.Response.ContentType = GenerateContentType("/index.html");
                    context.Response.ContentLength64 = webPage.Length;
                    context.Response.OutputStream.Write(webPage, 0, webPage.Length);
                }
                else if (IsValidFile(context.Request.Url.AbsolutePath))
                {
                    var requestedFile = GetIotCoreBrowserFile(context.Request.Url.AbsolutePath);
                    context.Response.ContentType = GenerateContentType(context.Request.Url.AbsolutePath);
                    context.Response.ContentLength64 = requestedFile.Length;
                    context.Response.OutputStream.Write(requestedFile, 0, requestedFile.Length);
                }
                else if (context.Request.Url.AbsolutePath == "/favicon.ico")
                {
                    context.Response.ContentType = "image/x-icon";
                    var contentLength = Favicon.Resources.Favicon48x48.Length;
                    context.Response.ContentLength64 = contentLength;
                    context.Response.OutputStream.Write(Favicon.Resources.Favicon48x48, 0, contentLength);
                }
                else
                {
                    // Data with get request not handled yet
                    var requestMessage = new Message(RequestCodes.Request, 1, context.Request.Url.AbsolutePath, null);
                    var responseMessage = RaiseRequestReceived(requestMessage);
                    var responseString = Converter.Serialize(responseMessage);
                    context.Response.AddHeader("Access-Control-Allow-Origin", "*");
                    context.Response.ContentType = Converter.ContentType;
                    var bytesToSend = Encoding.UTF8.GetBytes(responseString);
                    context.Response.OutputStream.Write(bytesToSend, 0, bytesToSend.Length);
                }
            }
            else if (context.Request.HttpMethod == "POST")
            {
                if (context.Request.HasEntityBody)
                {
                    var inputStream = new MemoryStream();
                    context.Request.InputStream.CopyTo(inputStream);
                    var requestString = Encoding.UTF8.GetString(inputStream.ToArray());
                    var message = Converter.Deserialize(requestString);
                    byte[] bytesToSend;
                    switch (message.Code)
                    {
                        case RequestCodes.Request:
                            {
                                var responseMessage = RaiseRequestReceived(message);
                                var responseString = Converter.Serialize(responseMessage);
                                bytesToSend = Encoding.UTF8.GetBytes(responseString);
                                break;
                            }
                        case RequestCodes.Event:
                            RaiseEventReceived(message);
                            bytesToSend = Array.Empty<byte>();
                            break;
                        default:
                            throw new IoTCoreException(ResponseCodes.BadRequest, $"Invalid message code {message.Code}");
                    }
                    context.Response.ContentType = Converter.ContentType;
                    context.Response.OutputStream.Write(bytesToSend, 0, bytesToSend.Length);
                }
                else
                {
                    throw new HttpListenerException((int)HttpStatusCode.BadRequest);
                }
            }
            else
            {
                throw new HttpListenerException((int)HttpStatusCode.BadRequest);
            }
        }
        catch (Exception e)
        {
            Message responseMessage;
            if (e is IoTCoreException ioTCoreException)
            {
                responseMessage = new Message(ioTCoreException.ResponseCode,
                    0,
                    null,
                    CreateErrorResponse(ioTCoreException.Message, ioTCoreException.ErrorCode, ioTCoreException.ErrorDetails));
            }
            else if (e is HttpListenerException listenerException)
            {
                context.Response.StatusCode = listenerException.ErrorCode is < 100 or > 999 ? 500 : listenerException.ErrorCode;
                responseMessage = new Message(listenerException.ErrorCode,
                    0,
                    null,
                    CreateErrorResponse(listenerException.Message, listenerException.ErrorCode));
            }
            else if (e is AggregateException aggregateException)
            {
                var errorMessages = new List<string>();
                if (aggregateException.InnerException?.Message != null)
                {
                    errorMessages.Add(aggregateException.InnerException.Message);
                }
                if (aggregateException.InnerExceptions.Any())
                {
                    errorMessages.AddRange(aggregateException.InnerExceptions.Select(innerException => innerException.Message));
                }
                responseMessage = new Message(ResponseCodes.InternalError,
                    0,
                    null,
                    CreateErrorResponse(aggregateException.Message, ResponseCodes.InternalError, string.Join(",", errorMessages)));
            }
            else
            {
                context.Response.StatusCode = 500;
                responseMessage = new Message(ResponseCodes.InternalError,
                    0,
                    null,
                    CreateErrorResponse(e.Message, ResponseCodes.InternalError));
            }

            var responseString = Converter.Serialize(responseMessage);
            context.Response.ContentType = Converter.ContentType;
            var bytesToSend = Encoding.UTF8.GetBytes(responseString);
            context.Response.OutputStream.Write(bytesToSend, 0, bytesToSend.Length);
        }

        context.Response.OutputStream.Close();
    }

    private static string GenerateContentType(string filePath)
    {
        string fileType = Path.GetExtension(filePath);
        //cut starting '.' away
        fileType = fileType.Substring(1);
        string contentType = "";
        switch (fileType)
        {
            case "htm": goto case "html";
            case "shtml": goto case "html";
            case "html": contentType = "text/html"; break;
            case "css": contentType = "text/css"; break;
            case "xml": contentType = "text/xml"; break;
            case "gif": contentType = "image/gif"; break;
            case "jpg": goto case "jpeg";
            case "jpeg": contentType = "image/jpeg"; break;
            case "js": contentType = "application/javascript"; break;
            case "atom": contentType = "application/atom+xml"; break;
            case "rss": contentType = "application/rss+xml"; break;

            case "mml": contentType = "text/mathml"; break;
            case "txt": contentType = "text/plain"; break;
            case "jad": contentType = "text/vnd.sun.j2me.app-descriptor"; break;
            case "wml": contentType = "text/vnd.wap.wml"; break;
            case "htc": contentType = "text/x-component"; break;

            case "avif": contentType = "image/avif"; break;
            case "png": contentType = "image/png"; break;
            case "svgz": goto case "svg";
            case "svg": contentType = "image/svg+xml"; break;
            case "tif": goto case "tiff";
            case "tiff": contentType = "image/tiff"; break;
            case "wbmp": contentType = "image/vnd.wap.wbmp"; break;
            case "webp": contentType = "image/webp"; break;
            case "ico": contentType = "image/x-icon"; break;
            case "jng": contentType = "image/x-jng"; break;
            case "bmp": contentType = "image/x-ms-bmp"; break;

            case "woff": contentType = "font/woff"; break;
            case "woff2": contentType = "image/woff2"; break;

            case "war": goto case "jar";
            case "ear": goto case "jar";
            case "jar": contentType = "application/java-archive"; break;
            case "json": contentType = "application/json"; break;
            case "hqx": contentType = "application/mac-binhex40"; break;
            case "doc": contentType = "application/msword"; break;
            case "pdf": contentType = "application/pdf"; break;
            case "eps": goto case "ps";
            case "ai": goto case "ps";
            case "ps": contentType = "application/postscript"; break;
            case "rtf": contentType = "application/rtf"; break;
            case "m3u8": contentType = "application/vnd.apple.mpegurl"; break;
            case "kml": contentType = "application/vnd.google-earth.kml+xml"; break;
            case "kmz": contentType = "application/vnd.google-earth.kmz"; break;
            case "xls": contentType = "application/vnd.ms-excel"; break;
            case "eot": contentType = "application/vnd.ms-fontobject"; break;
            case "ppt": contentType = "application/vnd.ms-powerpoint"; break;
            case "odg": contentType = "application/vnd.oasis.opendocument.graphics"; break;
            case "odp": contentType = "application/vnd.oasis.opendocument.presentation"; break;
            case "ods": contentType = "application/vnd.oasis.opendocument.spreadsheet"; break;
            case "odt": contentType = "application/vnd.oasis.opendocument.text"; break;
            case "pptx": contentType = "application/vnd.openxmlformats-officedocument.presentationml.presentation"; break;
            case "xlsx": contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"; break;
            case "docx": contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document"; break;
            case "wmlc": contentType = "application/vnd.wap.wmlc"; break;
            case "wasm": contentType = "application/wasm"; break;
            case "7z": contentType = "application/x-7z-compressed"; break;
            case "cco": contentType = "application/x-cocoa"; break;
            case "jardiff": contentType = "application/x-java-archive-diff"; break;
            case "jnlp": contentType = "application/x-java-jnlp-file"; break;
            case "run": contentType = "application/x-makeself"; break;
            case "pl": goto case "pm";
            case "pm": contentType = "application/x-perl"; break;
            case "prc": goto case "pdb";
            case "pdb": contentType = "application/x-pilot"; break;
            case "rar": contentType = "application/x-rar-compressed"; break;
            case "rpm": contentType = "application/x-redhat-package-manager"; break;
            case "sea": contentType = "application/x-sea"; break;
            case "swf": contentType = "application/x-shockwave-flash"; break;
            case "sit": contentType = "application/x-stuffit"; break;
            case "tcl": goto case "tcl";
            case "tk": contentType = "application/x-tcl"; break;
            case "der": goto case "crt";
            case "pem": goto case "crt";
            case "crt": contentType = "application/x-x509-ca-cert"; break;
            case "xpi": contentType = "application/x-xpinstall"; break;
            case "xhtml": contentType = "application/xhtml+xml"; break;
            case "xspf": contentType = "application/xspf+xml"; break;
            case "zip": contentType = "application/zip"; break;

            case "bin": goto case "dll";
            case "exe": goto case "dll";
            case "dll": contentType = "application/octet-stream"; break;
            case "deb": contentType = "application/octet-stream"; break;
            case "dmg": contentType = "application/octet-stream"; break;
            case "iso": goto case "img";
            case "img": contentType = "application/octet-stream"; break;
            case "msi": goto case "msm";
            case "msp": goto case "msm";
            case "msm": contentType = "application/octet-stream"; break;

            case "mid": goto case "kar";
            case "midi": goto case "kar";
            case "kar": contentType = "audio/midi"; break;
            case "mp3": contentType = "audio/mpeg"; break;
            case "ogg": contentType = "audio/ogg"; break;
            case "m4a": contentType = "audio/x-m4a"; break;
            case "ra": contentType = "audio/x-realaudio"; break;

            case "3gpp": goto case "3gp";
            case "3gp": contentType = "video/3gpp"; break;
            case "ts": contentType = "video/mp2t"; break;
            case "mp4": contentType = "video/mp4"; break;
            case "mpeg": goto case "mpg";
            case "mpg": contentType = "video/mpeg"; break;
            case "mov": contentType = "video/quicktime"; break;
            case "webm": contentType = "video/webm"; break;
            case "flv": contentType = "video/x-flv"; break;
            case "m4v": contentType = "video/x-m4v"; break;
            case "mng": contentType = "video/x-mng"; break;
            case "asx": goto case "asf";
            case "asf": contentType = "video/x-ms-asf"; break;
            case "wmv": contentType = "video/x-ms-wmv"; break;
            case "avi": contentType = "video/x-msvideo"; break;
        }
        return contentType;
    }

    private byte[] GetIotCoreBrowserFile(string filePath)
    {
        var fullPath = AppendLocalPath(filePath);
        using (var fileStream = File.Open(fullPath, FileMode.Open, FileAccess.Read))
        {
            var webPage = new byte[fileStream.Length];
            var bytesRead = 0;
            while (bytesRead < webPage.Length)
            {
                bytesRead += fileStream.Read(webPage, bytesRead, webPage.Length - bytesRead);
            }
            return webPage;
        }
    }

    private static bool IsValidFile(string filePath)
    {
        var localDirectory = AppendLocalPath(filePath);

        if (!File.Exists(localDirectory))
        {
            return false;
        }

        return CheckDirectoryTraversal(filePath, AppendLocalPath(""));
    }

    private static bool CheckDirectoryTraversal(string filePath, string validDirectory)
    {
        //(https://systemweakness.com/how-to-exploit-directory-traversal-vulnerabilities-176eeb7f2655)

        var validFiles = Directory.GetFiles(validDirectory, "*.*", SearchOption.AllDirectories);
        var cutFrom = validDirectory.Length;

        var result = validFiles.Any(aValidFile =>
        {
            var cuttedValidFile = aValidFile.Substring(cutFrom).Replace('\\', '/');
            return filePath.Equals(cuttedValidFile);
        });
        return result;
    }

    private static string AppendLocalPath(string absolutePath)
    {
        var baseDir = AppDomain.CurrentDomain.BaseDirectory;
        var parts = absolutePath.Split('/');
        string result = Path.Combine(baseDir, "Tools", "Visualizer", "ecologOne");

        foreach (var part in parts)
        {
            result = Path.Combine(result, part);
        }

        return result;
    }

    private static byte[] GetIotCoreBrowserWebPage()
    {
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tools", "Visualizer", "full.ifm-iot-core-visualizer.html.gz");
        return File.ReadAllBytes(path);
    }

    private Message RaiseRequestReceived(Message message)
    {
        var args = new IoTCore.NetAdapterManager.Contracts.RequestMessageEventArgs(message);
        RequestReceived?.Raise(this, args);
        return args.ResponseMessage;
    }

    private void RaiseEventReceived(Message message)
    {
        EventReceived?.Raise(this, new IoTCore.NetAdapterManager.Contracts.EventMessageEventArgs(message));
    }

    private static Variant CreateErrorResponse(string msg, int? code = null, string? details = null)
    {
        var ret = new VariantObject { { "msg", new VariantValue(msg) } };
        if (code != null)
        {
            ret.Add("code", new VariantValue(code.Value));
        }
        if (details != null)
        {
            ret.Add("details", new VariantValue(details));
        }
        return ret;
    }

    public void Dispose()
    {
        _disposed = true;
        _httpListener.Close();
        ((IDisposable)_httpListener).Dispose();
    }
}