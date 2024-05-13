namespace ifm.IoTCore.ElementManager.Contracts.Elements.Formats;

using Common.Variant;
using Valuations;

/// <summary>
/// Represents the format of an signed 8 bit integer type data element
/// </summary>
public class Int8Format : Format
{
    /// <summary>
    /// Gets the valuation.
    /// </summary>
    [VariantProperty("valuation", IgnoredIfNull = true)]
    public Int8Valuation Valuation { get; set; }

    /// <summary>
    /// The parameterless constructor for the variant converter.
    /// </summary>
    [VariantConstructor]
    public Int8Format() : base(Types.Number, Encodings.Int8, NameSpaces.Json)
    {
    }

    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="valuation">The valuation.</param>
    /// <param name="ns">The namespace.</param>
    public Int8Format(Int8Valuation valuation = null, string ns = NameSpaces.Json) : base(Types.Number, Encodings.Int8, ns)
    {
        Valuation = valuation;
    }
}

/// <summary>
/// Represents the format of an unsigned 8 bit integer type data element
/// </summary>
public class UInt8Format : Format
{
    /// <summary>
    /// Gets the valuation.
    /// </summary>
    [VariantProperty("valuation", IgnoredIfNull = true)]
    public UInt8Valuation Valuation { get; set; }

    /// <summary>
    /// The parameterless constructor for the variant converter.
    /// </summary>
    [VariantConstructor]
    public UInt8Format() : base(Types.Number, Encodings.UInt8, NameSpaces.Json)
    {
    }

    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="valuation">The valuation.</param>
    /// <param name="ns">The namespace.</param>
    public UInt8Format(UInt8Valuation valuation = null, string ns = NameSpaces.Json) : base(Types.Number, Encodings.UInt8, ns)
    {
        Valuation = valuation;
    }
}

/// <summary>
/// Represents the format of an signed 16 bit integer type data element
/// </summary>
public class Int16Format : Format
{
    /// <summary>
    /// Gets the valuation.
    /// </summary>
    [VariantProperty("valuation", IgnoredIfNull = true)]
    public Int16Valuation Valuation { get; set; }

    /// <summary>
    /// The parameterless constructor for the variant converter.
    /// </summary>
    [VariantConstructor]
    public Int16Format() : base(Types.Number, Encodings.Int16, NameSpaces.Json)
    {
    }

    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="valuation">The valuation.</param>
    /// <param name="ns">The namespace.</param>
    public Int16Format(Int16Valuation valuation = null, string ns = NameSpaces.Json) : base(Types.Number, Encodings.Int16, ns)
    {
        Valuation = valuation;
    }
}

/// <summary>
/// Represents the format of an unsigned 16 bit integer type data element
/// </summary>
public class UInt16Format : Format
{
    /// <summary>
    /// Gets the valuation.
    /// </summary>
    [VariantProperty("valuation", IgnoredIfNull = true)]
    public UInt16Valuation Valuation { get; set; }

    /// <summary>
    /// The parameterless constructor for the variant converter.
    /// </summary>
    [VariantConstructor]
    public UInt16Format() : base(Types.Number, Encodings.UInt16, NameSpaces.Json)
    {
    }

    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="valuation">The valuation.</param>
    /// <param name="ns">The namespace.</param>
    public UInt16Format(UInt16Valuation valuation = null, string ns = NameSpaces.Json) : base(Types.Number, Encodings.UInt16, ns)
    {
        Valuation = valuation;
    }
}

/// <summary>
/// Represents the format of an signed 32 bit integer type data element
/// </summary>
public class Int32Format : Format
{
    /// <summary>
    /// Gets the valuation.
    /// </summary>
    [VariantProperty("valuation", IgnoredIfNull = true)]
    public Int32Valuation Valuation { get; set; }

    /// <summary>
    /// The parameterless constructor for the variant converter.
    /// </summary>
    [VariantConstructor]
    public Int32Format() : base(Types.Number, Encodings.Int32, NameSpaces.Json)
    {
    }

    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="valuation">The valuation.</param>
    /// <param name="ns">The namespace.</param>
    public Int32Format(Int32Valuation valuation = null, string ns = NameSpaces.Json) : base(Types.Number, Encodings.Int32, ns)
    {
        Valuation = valuation;
    }
}

/// <summary>
/// Represents the format of an unsigned 32 bit integer type data element
/// </summary>
public class UInt32Format : Format
{
    /// <summary>
    /// Gets the valuation.
    /// </summary>
    [VariantProperty("valuation", IgnoredIfNull = true)]
    public UInt32Valuation Valuation { get; set; }

    /// <summary>
    /// The parameterless constructor for the variant converter.
    /// </summary>
    [VariantConstructor]
    public UInt32Format() : base(Types.Number, Encodings.UInt32, NameSpaces.Json)
    {
    }

    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="valuation">The valuation.</param>
    /// <param name="ns">The namespace.</param>
    public UInt32Format(UInt32Valuation valuation = null, string ns = NameSpaces.Json) : base(Types.Number, Encodings.UInt32, ns)
    {
        Valuation = valuation;
    }
}

/// <summary>
/// Represents the format of an signed 64 bit integer type data element
/// </summary>
public class Int64Format : Format
{
    /// <summary>
    /// Gets the valuation.
    /// </summary>
    [VariantProperty("valuation", IgnoredIfNull = true)]
    public Int64Valuation Valuation { get; set; }

    /// <summary>
    /// The parameterless constructor for the variant converter.
    /// </summary>
    [VariantConstructor]
    public Int64Format() : base(Types.Number, Encodings.Int64, NameSpaces.Json)
    {
    }

    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="valuation">The valuation.</param>
    /// <param name="ns">The namespace.</param>
    public Int64Format(Int64Valuation valuation = null, string ns = NameSpaces.Json) : base(Types.Number, Encodings.Int64, ns)
    {
        Valuation = valuation;
    }
}

/// <summary>
/// Represents the format of an unsigned 64 bit integer type data element
/// </summary>
public class UInt64Format : Format
{
    /// <summary>
    /// Gets the valuation.
    /// </summary>
    [VariantProperty("valuation", IgnoredIfNull = true)]
    public UInt64Valuation Valuation { get; set; }

    /// <summary>
    /// The parameterless constructor for the variant converter.
    /// </summary>
    [VariantConstructor]
    public UInt64Format() : base(Types.Number, Encodings.UInt64, NameSpaces.Json)
    {
    }

    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="valuation">The valuation.</param>
    /// <param name="ns">The namespace.</param>
    public UInt64Format(UInt64Valuation valuation = null, string ns = NameSpaces.Json) : base(Types.Number, Encodings.UInt64, ns)
    {
        Valuation = valuation;
    }
}

/// <summary>
/// Represents the format of an signed 32 bit integer type data element
/// </summary>
public class IntegerFormat : Format
{
    /// <summary>
    /// Gets the valuation.
    /// </summary>
    [VariantProperty("valuation", IgnoredIfNull = true)]
    public IntegerValuation Valuation { get; set; }

    /// <summary>
    /// The parameterless constructor for the variant converter.
    /// </summary>
    [VariantConstructor]
    public IntegerFormat() : base(Types.Number, Encodings.Int32, NameSpaces.Json)
    {
    }

    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="valuation">The valuation.</param>
    /// <param name="ns">The namespace.</param>
    public IntegerFormat(IntegerValuation valuation = null, string ns = NameSpaces.Json) : base(Types.Number, Encodings.Integer, ns)
    {
        Valuation = valuation;
    }
}
