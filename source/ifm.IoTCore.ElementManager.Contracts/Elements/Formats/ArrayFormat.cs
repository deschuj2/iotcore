namespace ifm.IoTCore.ElementManager.Contracts.Elements.Formats;

using Common.Variant;
using Valuations;

/// <summary>
/// Represents the format of an array type data element.
/// </summary>
public class ArrayFormat : Format
{
    /// <summary>
    /// Gets the valuation.
    /// </summary>
    [VariantProperty("valuation", IgnoredIfNull = true)]
    public ArrayValuation Valuation { get; set; }

    /// <summary>
    /// The parameterless constructor for the variant converter.
    /// </summary>
    [VariantConstructor]
    public ArrayFormat() : base(Types.Array, null, NameSpaces.Json)
    {
    }

    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="valuation">The valuation.</param>
    /// <param name="ns">The namespace.</param>
    public ArrayFormat(ArrayValuation valuation = null, string ns = NameSpaces.Json) : base(Types.Array, null, ns)
    {
        Valuation = valuation;
    }
}