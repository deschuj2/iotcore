namespace ifm.IoTCore.ElementManager.Contracts.Elements.Formats;

using Common.Variant;
using Valuations;

/// <summary>
/// Represents the format of an object type data element.
/// </summary>
public class ObjectFormat : Format
{
    /// <summary>
    /// Gets or sets the valuation.
    /// </summary>
    [VariantProperty("valuation", IgnoredIfNull = true)]
    public ObjectValuation Valuation { get; set; }

    /// <summary>
    /// The parameterless constructor for the variant converter.
    /// </summary>
    [VariantConstructor]
    public ObjectFormat() : base(Types.Object, null, NameSpaces.Json)
    {
    }

    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="valuation">The valuation.</param>
    /// <param name="ns">The namespace.</param>
    public ObjectFormat(ObjectValuation valuation = null, string ns = NameSpaces.Json) : base(Types.Object, null, ns)
    {
        Valuation = valuation;
    }
}