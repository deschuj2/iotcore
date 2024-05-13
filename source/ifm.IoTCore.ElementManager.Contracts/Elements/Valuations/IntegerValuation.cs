namespace ifm.IoTCore.ElementManager.Contracts.Elements.Valuations;

using System.Collections.Generic;
using Common.Variant;

/// <summary>
/// Represents the valuation for an signed 8 bit integer type data element.
/// </summary>
public class Int8Valuation : NumberValuation<sbyte>
{
    /// <summary>
    /// The parameterless constructor for the variant converter.
    /// </summary>
    [VariantConstructor]
    public Int8Valuation()
    {
    }

    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <param name="singleValues">The single values.</param>
    /// <param name="valueRanges">The value ranges.</param>
    /// <param name="gradient">The gradient.</param>
    /// <param name="offset">The offset.</param>
    /// <param name="displayFormat">The display format.</param>
    public Int8Valuation(sbyte? min,
        sbyte? max,
        sbyte? defaultValue = null,
        List<SingleValue<sbyte>> singleValues = null,
        List<ValueRange<sbyte>> valueRanges = null,
        double? gradient = null,
        double? offset = null,
        string displayFormat = null) : base(min, max, defaultValue, singleValues, valueRanges, gradient, offset, displayFormat)
    {
    }
}

/// <summary>
/// Represents the valuation for an unsigned 8 bit integer type data element.
/// </summary>
public class UInt8Valuation : NumberValuation<byte>
{
    /// <summary>
    /// The parameterless constructor for the variant converter.
    /// </summary>
    [VariantConstructor]
    public UInt8Valuation()
    {
    }

    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <param name="singleValues">The single values.</param>
    /// <param name="valueRanges">The value ranges.</param>
    /// <param name="gradient">The gradient.</param>
    /// <param name="offset">The offset.</param>
    /// <param name="displayFormat">The display format.</param>
    public UInt8Valuation(byte? min,
        byte? max,
        byte? defaultValue = null,
        List<SingleValue<byte>> singleValues = null,
        List<ValueRange<byte>> valueRanges = null,
        double? gradient = null,
        double? offset = null,
        string displayFormat = null) : base(min, max, defaultValue, singleValues, valueRanges, gradient, offset, displayFormat)
    {
    }
}

/// <summary>
/// Represents the valuation for an signed 16 bit integer type data element.
/// </summary>
public class Int16Valuation : NumberValuation<short>
{
    /// <summary>
    /// The parameterless constructor for the variant converter.
    /// </summary>
    [VariantConstructor]
    public Int16Valuation()
    {
    }

    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <param name="singleValues">The single values.</param>
    /// <param name="valueRanges">The value ranges.</param>
    /// <param name="gradient">The gradient.</param>
    /// <param name="offset">The offset.</param>
    /// <param name="displayFormat">The display format.</param>
    public Int16Valuation(short? min,
        short? max,
        short? defaultValue = null,
        List<SingleValue<short>> singleValues = null,
        List<ValueRange<short>> valueRanges = null,
        double? gradient = null,
        double? offset = null,
        string displayFormat = null) : base(min, max, defaultValue, singleValues, valueRanges, gradient, offset, displayFormat)
    {
    }
}

/// <summary>
/// Represents the valuation for an unsigned 16 bit integer type data element.
/// </summary>
public class UInt16Valuation : NumberValuation<ushort>
{
    /// <summary>
    /// The parameterless constructor for the variant converter.
    /// </summary>
    [VariantConstructor]
    public UInt16Valuation()
    {
    }

    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <param name="singleValues">The single values.</param>
    /// <param name="valueRanges">The value ranges.</param>
    /// <param name="gradient">The gradient.</param>
    /// <param name="offset">The offset.</param>
    /// <param name="displayFormat">The display format.</param>
    public UInt16Valuation(ushort? min,
        ushort? max,
        ushort? defaultValue = null,
        List<SingleValue<ushort>> singleValues = null,
        List<ValueRange<ushort>> valueRanges = null,
        double? gradient = null,
        double? offset = null,
        string displayFormat = null) : base(min, max, defaultValue, singleValues, valueRanges, gradient, offset, displayFormat)
    {
    }
}

/// <summary>
/// Represents the valuation for an signed 32 bit integer type data element.
/// </summary>
public class Int32Valuation : NumberValuation<int>
{
    /// <summary>
    /// The parameterless constructor for the variant converter.
    /// </summary>
    [VariantConstructor]
    public Int32Valuation()
    {
    }

    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <param name="singleValues">The single values.</param>
    /// <param name="valueRanges">The value ranges.</param>
    /// <param name="gradient">The gradient.</param>
    /// <param name="offset">The offset.</param>
    /// <param name="displayFormat">The display format.</param>
    public Int32Valuation(int? min,
        int? max,
        int? defaultValue = null,
        List<SingleValue<int>> singleValues = null,
        List<ValueRange<int>> valueRanges = null,
        double? gradient = null,
        double? offset = null,
        string displayFormat = null) : base(min, max, defaultValue, singleValues, valueRanges, gradient, offset, displayFormat)
    {
    }
}

/// <summary>
/// Represents the valuation for an unsigned 32 bit integer type data element.
/// </summary>
public class UInt32Valuation : NumberValuation<uint>
{
    /// <summary>
    /// The parameterless constructor for the variant converter.
    /// </summary>
    [VariantConstructor]
    public UInt32Valuation()
    {
    }

    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <param name="singleValues">The single values.</param>
    /// <param name="valueRanges">The value ranges.</param>
    /// <param name="gradient">The gradient.</param>
    /// <param name="offset">The offset.</param>
    /// <param name="displayFormat">The display format.</param>
    public UInt32Valuation(uint? min,
        uint? max,
        uint? defaultValue = null,
        List<SingleValue<uint>> singleValues = null,
        List<ValueRange<uint>> valueRanges = null,
        double? gradient = null,
        double? offset = null,
        string displayFormat = null) : base(min, max, defaultValue, singleValues, valueRanges, gradient, offset, displayFormat)
    {
    }
}

/// <summary>
/// Represents the valuation for an signed 64 bit integer type data element.
/// </summary>
public class Int64Valuation : NumberValuation<long>
{
    /// <summary>
    /// The parameterless constructor for the variant converter.
    /// </summary>
    [VariantConstructor]
    public Int64Valuation()
    {
    }

    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <param name="singleValues">The single values.</param>
    /// <param name="valueRanges">The value ranges.</param>
    /// <param name="gradient">The gradient.</param>
    /// <param name="offset">The offset.</param>
    /// <param name="displayFormat">The display format.</param>
    public Int64Valuation(long? min,
        long? max,
        long? defaultValue = null,
        List<SingleValue<long>> singleValues = null,
        List<ValueRange<long>> valueRanges = null,
        double? gradient = null,
        double? offset = null,
        string displayFormat = null) : base(min, max, defaultValue, singleValues, valueRanges, gradient, offset, displayFormat)
    {
    }
}

/// <summary>
/// Represents the valuation for an unsigned 64 bit integer type data element.
/// </summary>
public class UInt64Valuation : NumberValuation<ulong>
{
    /// <summary>
    /// The parameterless constructor for the variant converter.
    /// </summary>
    [VariantConstructor]
    public UInt64Valuation()
    {
    }

    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <param name="singleValues">The single values.</param>
    /// <param name="valueRanges">The value ranges.</param>
    /// <param name="gradient">The gradient.</param>
    /// <param name="offset">The offset.</param>
    /// <param name="displayFormat">The display format.</param>
    public UInt64Valuation(ulong? min,
        ulong? max,
        ulong? defaultValue = null,
        List<SingleValue<ulong>> singleValues = null,
        List<ValueRange<ulong>> valueRanges = null,
        double? gradient = null,
        double? offset = null,
        string displayFormat = null) : base(min, max, defaultValue, singleValues, valueRanges, gradient, offset, displayFormat)
    {
    }
}

/// <summary>
/// Represents the valuation for an signed 32 bit integer type data element.
/// </summary>
public class IntegerValuation : NumberValuation<int>
{
    /// <summary>
    /// The parameterless constructor for the variant converter.
    /// </summary>
    [VariantConstructor]
    public IntegerValuation()
    {
    }

    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <param name="singleValues">The single values.</param>
    /// <param name="valueRanges">The value ranges.</param>
    /// <param name="gradient">The gradient.</param>
    /// <param name="offset">The offset.</param>
    /// <param name="displayFormat">The display format.</param>
    public IntegerValuation(int? min,
        int? max,
        int? defaultValue = null,
        List<SingleValue<int>> singleValues = null,
        List<ValueRange<int>> valueRanges = null,
        double? gradient = null,
        double? offset = null,
        string displayFormat = null) : base(min, max, defaultValue, singleValues, valueRanges, gradient, offset, displayFormat)
    {
    }
}
