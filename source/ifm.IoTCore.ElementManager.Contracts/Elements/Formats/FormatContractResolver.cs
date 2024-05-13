﻿namespace ifm.IoTCore.ElementManager.Contracts.Elements.Formats;

using Common.Variant;
using Valuations;

/// <summary>
/// Provides the contract resolver to convert between Variant and Format types.
/// </summary>
public class FormatContractResolver : IVariantContractResolver
{
    /// <summary>
    /// Creates a new Format object from the given Variant object.
    /// </summary>
    /// <param name="data">The Variant object to convert.</param>
    /// <returns>The newly created Format object.</returns>
    public object CreateInstance(Variant data)
    {
        if (data is VariantObject variantObject)
        {
            return CreateFormat(variantObject);
        }
        return null;
    }

    private static Format CreateFormat(VariantObject data)
    {
        var type = (string)data["type"].AsVariantValue();

        string encoding = null;
        if (data.TryGetValue("encoding", out var value))
        {
            encoding = (string)value.AsVariantValue();
        }

        string ns = null;
        if (data.TryGetValue("namespace", out value))
        {
            ns = (string)value.AsVariantValue();
        }

        switch (type)
        {
            case "boolean":
                return CreateBooleanFormat(data, ns);
            case "number":
                return CreateNumberFormat(data, encoding, ns);
            case "string":
                return CreateStringFormat(data, encoding, ns);
            case "enum":
                return CreateIntegerEnumFormat(data, ns);
            case "array":
                return CreateArrayFormat(data, ns);
            case "object":
                return CreateObjectFormat(data, ns);
            default:
                return null;
        }
    }

    private static BooleanFormat CreateBooleanFormat(VariantObject data, string ns)
    {
        BooleanValuation valuation = null;
        if (data.TryGetValue("valuation", out var value))
        {
            valuation = value.ToObject<BooleanValuation>();
        }
        return new BooleanFormat(valuation, ns);
    }

    private static Format CreateNumberFormat(VariantObject data, string encoding, string ns)
    {
        return encoding switch
        {
            Format.Encodings.Int8 => CreateInt8Format(data, ns),
            Format.Encodings.UInt8 => CreateUInt8Format(data, ns),
            Format.Encodings.Int16 => CreateInt16Format(data, ns),
            Format.Encodings.UInt16 => CreateUInt16Format(data, ns),
            Format.Encodings.Int32 => CreateInt32Format(data, ns),
            Format.Encodings.Integer => CreateIntegerFormat(data, ns),
            Format.Encodings.UInt32 => CreateUInt32Format(data, ns),
            Format.Encodings.Int64 => CreateInt64Format(data, ns),
            Format.Encodings.UInt64 => CreateUInt64Format(data, ns),
            Format.Encodings.Float => CreateFloatFormat(data, ns),
            Format.Encodings.Double => CreateDoubleFormat(data, ns),
            _ => null
        };
    }

    private static Int8Format CreateInt8Format(VariantObject data, string ns)
    {
        Int8Valuation valuation = null;
        if (data.TryGetValue("valuation", out var value))
        {
            valuation = value.ToObject<Int8Valuation>();
        }
        return new Int8Format(valuation, ns);
    }

    private static UInt8Format CreateUInt8Format(VariantObject data, string ns)
    {
        UInt8Valuation valuation = null;
        if (data.TryGetValue("valuation", out var value))
        {
            valuation = value.ToObject<UInt8Valuation>();
        }
        return new UInt8Format(valuation, ns);
    }

    private static Int16Format CreateInt16Format(VariantObject data, string ns)
    {
        Int16Valuation valuation = null;
        if (data.TryGetValue("valuation", out var value))
        {
            valuation = value.ToObject<Int16Valuation>();
        }
        return new Int16Format(valuation, ns);
    }

    private static UInt16Format CreateUInt16Format(VariantObject data, string ns)
    {
        UInt16Valuation valuation = null;
        if (data.TryGetValue("valuation", out var value))
        {
            valuation = value.ToObject<UInt16Valuation>();
        }
        return new UInt16Format(valuation, ns);
    }

    private static Int32Format CreateInt32Format(VariantObject data, string ns)
    {
        Int32Valuation valuation = null;
        if (data.TryGetValue("valuation", out var value))
        {
            valuation = value.ToObject<Int32Valuation>();
        }
        return new Int32Format(valuation, ns);
    }

    private static IntegerFormat CreateIntegerFormat(VariantObject data, string ns)
    {
        IntegerValuation valuation = null;
        if (data.TryGetValue("valuation", out var value))
        {
            valuation = value.ToObject<IntegerValuation>();
        }
        return new IntegerFormat(valuation, ns);
    }

    private static UInt32Format CreateUInt32Format(VariantObject data, string ns)
    {
        UInt32Valuation valuation = null;
        if (data.TryGetValue("valuation", out var value))
        {
            valuation = value.ToObject<UInt32Valuation>();
        }
        return new UInt32Format(valuation, ns);
    }

    private static Int64Format CreateInt64Format(VariantObject data, string ns)
    {
        Int64Valuation valuation = null;
        if (data.TryGetValue("valuation", out var value))
        {
            valuation = value.ToObject<Int64Valuation>();
        }
        return new Int64Format(valuation, ns);
    }

    private static UInt64Format CreateUInt64Format(VariantObject data, string ns)
    {
        UInt64Valuation valuation = null;
        if (data.TryGetValue("valuation", out var value))
        {
            valuation = value.ToObject<UInt64Valuation>();
        }
        return new UInt64Format(valuation, ns);
    }

    private static FloatFormat CreateFloatFormat(VariantObject data, string ns)
    {
        FloatValuation valuation = null;
        if (data.TryGetValue("valuation", out var value))
        {
            valuation = value.ToObject<FloatValuation>();
        }
        return new FloatFormat(valuation, ns);
    }

    private static DoubleFormat CreateDoubleFormat(VariantObject data, string ns)
    {
        DoubleValuation valuation = null;
        if (data.TryGetValue("valuation", out var value))
        {
            valuation = value.ToObject<DoubleValuation>();
        }
        return new DoubleFormat(valuation, ns);
    }

    private static StringFormat CreateStringFormat(VariantObject data, string encoding, string ns)
    {
        StringValuation valuation = null;
        if (data.TryGetValue("valuation", out var value))
        {
            valuation = value.ToObject<StringValuation>();
        }
        return new StringFormat(valuation, encoding, ns);
    }

    private static IntegerEnumFormat CreateIntegerEnumFormat(VariantObject data, string ns)
    {
        IntegerEnumValuation valuation = null;
        if (data.TryGetValue("valuation", out var value))
        {
            valuation = value.ToObject<IntegerEnumValuation>();
        }
        return new IntegerEnumFormat(valuation, ns);
    }

    private static ArrayFormat CreateArrayFormat(VariantObject data, string ns)
    {
        ArrayValuation valuation = null;
        if (data.TryGetValue("valuation", out var value))
        {
            valuation = value.ToObject<ArrayValuation>();
        }
        return new ArrayFormat(valuation, ns);
    }

    private static ObjectFormat CreateObjectFormat(VariantObject data, string ns)
    {
        ObjectValuation valuation = null;
        if (data.TryGetValue("valuation", out var value))
        {
            valuation = value.ToObject<ObjectValuation>();
        }
        return new ObjectFormat(valuation, ns);
    }
}

