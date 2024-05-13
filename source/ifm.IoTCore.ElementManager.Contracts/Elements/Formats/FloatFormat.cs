namespace ifm.IoTCore.ElementManager.Contracts.Elements.Formats;

using Common.Variant;
using Valuations;

/// <summary>
/// Represents the format of a float type data element.
/// </summary>
public class FloatFormat : Format
{
    /// <summary>
    /// Gets the valuation.
    /// </summary>
    [VariantProperty("valuation", IgnoredIfNull = true)]
    public FloatValuation Valuation { get; set; }

    /// <summary>
    /// The parameterless constructor for the variant converter.
    /// </summary>
    [VariantConstructor]
    public FloatFormat() : base(Types.Number, Encodings.Float, NameSpaces.Json)
    {
    }

    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="valuation">The valuation.</param>
    /// <param name="ns">The namespace.</param>
    public FloatFormat(FloatValuation valuation = null, string ns = NameSpaces.Json) : base(Types.Number, Encodings.Float, ns)
    {
        Valuation = valuation;
    }
}

/// <summary>
/// Represents the format of a double type data element.
/// </summary>
public class DoubleFormat : Format
{
    /// <summary>
    /// Gets the valuation
    /// </summary>
    [VariantProperty("valuation", IgnoredIfNull = true)]
    public DoubleValuation Valuation { get; set; }

    /// <summary>
    /// The parameterless constructor for the variant converter.
    /// </summary>
    [VariantConstructor]
    public DoubleFormat() : base(Types.Number, Encodings.Double, NameSpaces.Json)
    {
    }

    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="valuation">The valuation.</param>
    /// <param name="ns">The namespace.</param>
    public DoubleFormat(DoubleValuation valuation = null, string ns = NameSpaces.Json) : base(Types.Number, Encodings.Double, ns)
    {
        Valuation = valuation;
    }
}