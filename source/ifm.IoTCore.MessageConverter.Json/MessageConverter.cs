namespace ifm.IoTCore.MessageConverter.Json;

using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Common;
using Common.Exceptions;
using Common.Variant;
using Contracts;
using Message;

[Flags]
public enum MessageDeserializationMode
{
    Default = 0,
    ThrowOnNull = 1
}

public class MessageConverter : IMessageConverter
{
    // Note: The encoder option UnsafeRelaxedJsonEscaping is needed because of special character escaping.
    // see : https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/character-encoding
    private readonly JsonSerializerOptions _serializeOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, 
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    private readonly MessageDeserializationMode _converterMode;

    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    public MessageConverter() : this(MessageDeserializationMode.Default)
    {
    }

    /// <summary>
    /// Initializes a new instance.
    /// <param name="messageConverterMode">The mode of the </param>
    /// </summary>
    public MessageConverter(MessageDeserializationMode messageConverterMode)
    {
        _converterMode = messageConverterMode;
    }

    public string Type => "json";

    public string ContentType => "application/json";

    public string Serialize(Message message)
    {
        if (message == null) throw new ArgumentNullException(nameof(message));

        try
        {
            var authenticationInfoConverter = message.Authentication != null ?
                new InnerAuthenticationInfo(message.Authentication.User, message.Authentication.Password) :
                null;

            JsonElement? data = message.Data != null ? 
                VariantConverter.ToJsonElement(message.Data) : 
                null;

            var convertMessage = new InnerMessage(message.Code,
                message.Cid,
                message.Address,
                data,
                message.Reply,
                authenticationInfoConverter);

            var json = JsonSerializer.Serialize(convertMessage, _serializeOptions);

            return json;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public Message Deserialize(string json)
    {
        if (json == null) throw new BadRequestException(nameof(json));

        try
        {
            var convertMessage = JsonSerializer.Deserialize<InnerMessage>(json) ?? throw new BadRequestException("Convert json failed");
            Variant data = null;
            try
            {
                if (convertMessage.Data.HasValue)
                {
                    data = VariantConverter.FromJsonElement(convertMessage.Data.Value, _converterMode.HasFlag(MessageDeserializationMode.ThrowOnNull));
                }
            }
            catch (Exception e)
            {
                throw new DataInvalidException(e.Message);
            }

            // Handle non-conform error response message
            if (!RequestCodes.IsRequestOrEvent(convertMessage.Code) && !ResponseCodes.IsSuccess(convertMessage.Code))
            {
                if (convertMessage.Error != null || convertMessage.Msg != null)
                {
                    data = new VariantObject();
                    if (convertMessage.Error != null) ((VariantObject)data).Add((VariantValue)"error", (VariantValue)convertMessage.Error);
                    if (convertMessage.Msg != null) ((VariantObject)data).Add((VariantValue)"msg", (VariantValue)convertMessage.Msg);
                }
            }

            var authenticationInfo = convertMessage.AuthenticationInfoConverter != null
                ? new Message.AuthenticationInfo(convertMessage.AuthenticationInfoConverter.User,
                    convertMessage.AuthenticationInfoConverter.Password)
                : null;

            var message = new Message(convertMessage.Code,
                convertMessage.Cid,
                convertMessage.Address,
                data,
                convertMessage.Reply,
                authenticationInfo);

            return message;
        }
        catch (BadRequestException)
        {
            throw;
        }
        catch (DataInvalidException)
        {
            throw;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    private class InnerAuthenticationInfo
    {
        [JsonPropertyName("user" )]
        public string User { get; }

        [JsonPropertyName("passwd" )]
        public string Password { get; }

        public InnerAuthenticationInfo(string user, string password)
        {
            User = user;
            Password = password;
        }
    }

    private class InnerMessage
    {
        [JsonIgnore]
        public int Code
        {
            get => InnerCode.GetInt32();
            set
            {
                var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(value.ToString()));
                InnerCode = JsonElement.ParseValue(ref reader);
            }
        }

        [JsonPropertyName("code"), JsonRequired] 
        public JsonElement InnerCode { get; set; }

        [JsonPropertyName("cid"), JsonRequired]
        public int Cid { get; set; }

        // Allow adr field to be null, because older iotcores do not have this in the response message.
        [JsonPropertyName("adr")]
        public string Address { get; set; }

        [JsonPropertyName("data")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public JsonElement? Data { get; set; }

        [JsonPropertyName("reply")]
        public string Reply { get; set; }

        [JsonPropertyName("auth")]
        public InnerAuthenticationInfo AuthenticationInfoConverter { get; set; }

        [JsonPropertyName("error")]
        public string Error { get; set; }

        [JsonPropertyName("msg")]
        public string Msg { get; set; }

        public InnerMessage(int code, int cid, string address, JsonElement? data, string reply, InnerAuthenticationInfo authenticationInfoConverter, string error = null, string msg = null)
        {
            Code = code;
            Cid = cid;
            Address = address;
            Data = data;
            Reply = reply;
            AuthenticationInfoConverter = authenticationInfoConverter;
            Error = error;
            Msg = msg;
        }

        [JsonConstructor]
        public InnerMessage(JsonElement innerCode, int cid, string address, JsonElement? data, string reply, InnerAuthenticationInfo authenticationInfoConverter, string error = null, string msg = null)
        {
            switch (innerCode.ValueKind)
            {
                case JsonValueKind.String:
                {
                    var codeAsString = innerCode.GetString();
                    switch (codeAsString?.ToLower())
                    {
                        case "event":
                            Code = RequestCodes.Event;
                            break;
                        case "request":
                            Code = RequestCodes.Request;
                            break;
                        default:
                            throw new BadRequestException($"Bad code '{codeAsString}'");
                    }

                    break;
                }
                case JsonValueKind.Number:
                {
                    Code = innerCode.GetInt32();
                    break;
                }
                case JsonValueKind.Undefined:
                    break;
                case JsonValueKind.Null:
                    throw new BadRequestException("Bad code 'null' not allowed");
                case JsonValueKind.Object:
                    throw new BadRequestException("Bad code 'object' not allowed");
                case JsonValueKind.Array:
                    throw new BadRequestException("Bad code 'array' not allowed");
                case JsonValueKind.True:
                    throw new BadRequestException("Bad code 'true' not allowed");
                case JsonValueKind.False:
                    throw new BadRequestException("Bad code 'false' not allowed");
                default:
                    throw new BadRequestException("Bad code 'unknown type' not allowed");
            }

            Cid = cid;
            Address = address;
            Data = data;
            Reply = reply;
            AuthenticationInfoConverter = authenticationInfoConverter;
            Error = error;
            Msg = msg;
        }
    }
}