namespace ifm.IoTCore.DataStore;

using System;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

internal class DoubleJsonNetFrameworkConverter : JsonConverter<double>
{
    public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
    {
        var encValue = Encoding.UTF8.GetBytes(value.ToString(CultureInfo.InvariantCulture));
        writer.WriteRawValue(new ReadOnlySpan<byte>(encValue));
    }
}