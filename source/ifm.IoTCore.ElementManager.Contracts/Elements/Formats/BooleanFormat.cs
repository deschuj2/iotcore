namespace ifm.IoTCore.ElementManager.Contracts.Elements.Formats;

using Common.Variant;
using Valuations;

/// <summary>
/// Represents the format of a boolean type data element.
/// </summary>
public class BooleanFormat : Format
{
    /// <summary>
    /// Gets the valuation.
    /// </summary>
    [VariantProperty("valuation", IgnoredIfNull = true)]
    public BooleanValuation Valuation { get; set; }

    /// <summary>
    /// The parameterless constructor for the variant converter.
    /// </summary>
    [VariantConstructor]
    public BooleanFormat() : base(Types.Boolean, null, NameSpaces.Json)
    {
    }

    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="valuation">The valuation.</param>
    /// <param name="ns">The namespace.</param>
    public BooleanFormat(BooleanValuation valuation = null, string ns = NameSpaces.Json) : base(Types.Boolean, null, ns)
    {
        Valuation = valuation;
    }
}