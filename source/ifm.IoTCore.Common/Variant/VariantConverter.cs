namespace ifm.IoTCore.Common.Variant;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

public class VariantConverter
{
    public static string ToJsonString(Variant variant)
    {
        return ToJsonElement(variant).ToString();
    }

    public static Variant FromJsonString(string json)
    {
        return FromJsonElement(JsonDocument.Parse(json).RootElement, false);
    }

    public static JsonElement ToJsonElement(Variant variant)
    {
        using (var memoryStream = new MemoryStream())
        {
            using (var writer = new Utf8JsonWriter(memoryStream))
            {
                WriteToUtf8Writer(writer, variant);

                writer.Flush();
                return JsonDocument.Parse(Encoding.UTF8.GetString(memoryStream.ToArray())).RootElement;
            }
        }
    }

    public static Variant FromJsonElement(JsonElement jsonElement, bool throwOnReadingNull)
    {
        if (jsonElement.ValueKind == JsonValueKind.Undefined)
        {
            return null;
        }

        return FromUtf8Reader(new Utf8JsonReader(Encoding.UTF8.GetBytes(jsonElement.GetRawText())), throwOnReadingNull);
    }

    private static void WriteToUtf8Writer(Utf8JsonWriter writer, VariantObject variantObject)
    {
        writer.WriteStartObject();

        foreach (var item in variantObject)
        {
            writer.WritePropertyName((string)(VariantValue)item.Key);
            WriteToUtf8Writer(writer, item.Value);
        }

        writer.WriteEndObject();
    }

    private static void WriteToUtf8Writer(Utf8JsonWriter writer, VariantArray variantArray)
    {
        writer.WriteStartArray();

        foreach (var item in variantArray)
        {
            WriteToUtf8Writer(writer, item);
        }

        writer.WriteEndArray();
    }

    private static void WriteToUtf8Writer(Utf8JsonWriter writer, VariantValue data)
    {
        switch (data.Type)
        {
            case VariantValue.ValueType.Boolean:
                writer.WriteBooleanValue((bool)data);
                break;
            case VariantValue.ValueType.Character:
                writer.WriteNumberValue((char)data);
                break;
            case VariantValue.ValueType.Int8:
                writer.WriteNumberValue((sbyte)data);
                break;
            case VariantValue.ValueType.UInt8:
                writer.WriteNumberValue((byte)data);
                break;
            case VariantValue.ValueType.Int16:
                writer.WriteNumberValue((short)data);
                break;
            case VariantValue.ValueType.UInt16:
                writer.WriteNumberValue((ushort)data);
                break;
            case VariantValue.ValueType.Int32:
                writer.WriteNumberValue((int)data);
                break;
            case VariantValue.ValueType.UInt32:
                writer.WriteNumberValue((uint)data);
                break;
            case VariantValue.ValueType.Int64:
                writer.WriteNumberValue((long)data);
                break;
            case VariantValue.ValueType.UInt64:
                writer.WriteNumberValue((ulong)data);
                break;
            case VariantValue.ValueType.Float:
                writer.WriteNumberValue((float)data);
                break;
            case VariantValue.ValueType.Double:
                writer.WriteNumberValue((double)data);
                break;
            case VariantValue.ValueType.Decimal:
                writer.WriteNumberValue((decimal)data);
                break;
            case VariantValue.ValueType.String:
                writer.WriteStringValue((string)data);
                break;
            case VariantValue.ValueType.DateTime:
                writer.WriteStringValue((DateTime)data);
                break;
            case VariantValue.ValueType.TimeSpan:
                writer.WriteStringValue(((TimeSpan)data).ToString());
                break;
            case VariantValue.ValueType.Uri:
                writer.WriteStringValue(((Uri)data).ToString());
                break;
            case VariantValue.ValueType.Guid:
                writer.WriteStringValue(((Guid)data).ToString());
                break;
            default:
                throw new Exception($"Unsupported variant value type {data.Type}");
        }
    }

    private static void WriteToUtf8Writer(Utf8JsonWriter writer, Variant variant)
    {
        if (variant is VariantObject variantObject)
        {
            WriteToUtf8Writer(writer, variantObject);
        }
        else if (variant is VariantArray variantArray)
        {
            WriteToUtf8Writer(writer, variantArray);
        }
        else if (variant is VariantValue variantValue)
        {
            WriteToUtf8Writer(writer, variantValue);
        }
        else if (variant == null)
        {
            writer.WriteNullValue();
        }
        else
        {
            throw new Exception("Unknown variant type");
        }
    }
        

    private static Variant FromUtf8Reader(Utf8JsonReader jsonReader, bool throwOnReadingNull)
    {
        var stack = new Stack<Variant>();
        var arrayStack = new Stack<VariantArray>();
        var objectStack = new Stack<VariantObject>();

        while (jsonReader.Read())
        {
            switch (jsonReader.TokenType)
            {
                case JsonTokenType.None:
                    break;

                case JsonTokenType.StartObject:

                    var obj = new VariantObject();
                    objectStack.Push(obj);
                    stack.Push(obj);

                    break;

                case JsonTokenType.EndObject:

                    var objectResult = objectStack.Pop();
                    var objectItem = stack.Pop();

                    Dictionary<string, Variant> variantDictionary = new();

                    while (objectItem != objectResult)
                    {
                        var propertyName = (string)(VariantValue)stack.Pop();

                        if (variantDictionary != null && variantDictionary.ContainsKey(propertyName))
                        {
                            throw new DuplicateKeyException($"The key '{propertyName}' has already been used in this object. Did you use duplicate keys in your json object?");
                        }

                        variantDictionary.Add(propertyName, objectItem);
                        objectItem = stack.Pop();
                    }

                    // The items need to be reversed, because they come in opposite order of the stack. (Last in first out).
                    foreach (var item in variantDictionary.Reverse())
                    {
                        objectResult.Add(item.Key, item.Value);
                    }

                    if (stack.Count == 0)
                    {
                        return objectResult;
                    }
                    else
                    {
                        stack.Push(objectResult);
                    }

                    break;

                case JsonTokenType.StartArray:

                    var array = new VariantArray();
                    arrayStack.Push(array);
                    stack.Push(array);

                    break;

                case JsonTokenType.EndArray:

                    var arrayResult = arrayStack.Pop();
                    var arrayItem = stack.Pop();

                    while (arrayItem != arrayResult)
                    {
                        arrayResult.Add(arrayItem);
                        arrayItem = stack.Pop();
                    }

                    // The items need to be reversed, because they come in opposite order of the stack. (Last in first out).
                    arrayResult.Reverse();

                    if (stack.Count == 0)
                    {
                        return arrayResult;
                    }
                    else
                    {
                        stack.Push(arrayResult);
                    }

                    break;

                case JsonTokenType.PropertyName:
                    stack.Push(new VariantValue(jsonReader.GetString()));
                    break;

                case JsonTokenType.Comment:
                    // Comments will be ignored.
                    break;

                case JsonTokenType.String:
                    stack.Push(new VariantValue(jsonReader.GetString()));
                    break;

                case JsonTokenType.Number:
                    stack.Push(new VariantValue(jsonReader.GetDouble()));
                    break;

                case JsonTokenType.True:
                case JsonTokenType.False:
                    stack.Push(new VariantValue(jsonReader.GetBoolean()));
                    break;

                case JsonTokenType.Null:
                    if (throwOnReadingNull)
                    {
                        throw new NullReadException("A null value has been read and throwOnReadingNull is set to true.");
                    }
                    stack.Push(null);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        if (stack.Count == 1)
        {
            return stack.Pop();
        }
        else
        {
            throw new Exception("Unhandled objects.");
        }

    }

    public class NullReadException : Exception
    {
        public NullReadException()
        {
        }

        public NullReadException(string message) : base(message)
        {
        }
    }

    public class DuplicateKeyException : Exception
    {
        public DuplicateKeyException()
        {
        }

        public DuplicateKeyException(string message) : base(message)
        {

        }
    }
}