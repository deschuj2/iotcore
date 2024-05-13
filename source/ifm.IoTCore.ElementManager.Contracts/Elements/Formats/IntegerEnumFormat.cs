namespace ifm.IoTCore.ElementManager.Contracts.Elements.Formats;

using Common.Variant;
using Valuations;

/// <summary>
/// Represents the format of an integer enumeration type data element.
/// </summary>
public class IntegerEnumFormat : Format
{
    /// <summary>
    /// Gets the valuation.
    /// </summary>
    [VariantProperty("valuation", IgnoredIfNull = true)]
    public IntegerEnumValuation Valuation { get; set; }

    /// <summary>
    /// The parameterless constructor for the variant converter.
    /// </summary>
    [VariantConstructor]
    public IntegerEnumFormat() : base(Types.Enum, Encodings.Integer, NameSpaces.Json)
    {
    }

    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="valuation">The valuation.</param>
    /// <param name="ns">The namespace.</param>
    public IntegerEnumFormat(IntegerEnumValuation valuation = null, string ns = NameSpaces.Json) : base(Types.Enum, Encodings.Integer, ns)
    {
        Valuation = valuation;
    }
}