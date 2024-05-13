namespace ifm.IoTCore.ElementManager.Contracts.Elements.Formats;

using Common.Variant;
using Valuations;

/// <summary>
/// Represents the format of a string type data element.
/// </summary>
public class StringFormat : Format
{
    /// <summary>
    /// Gets the valuation.
    /// </summary>
    [VariantProperty("valuation", IgnoredIfNull = true)]
    public StringValuation Valuation { get; set; }

    /// <summary>
    /// The parameterless constructor for the variant converter.
    /// </summary>
    [VariantConstructor]
    public StringFormat() : base(Types.String, Encodings.Utf8, NameSpaces.Json)
    {
    }

    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="valuation">The valuation.</param>
    /// <param name="encoding">The encoding.</param>
    /// <param name="ns">The namespace.</param>
    public StringFormat(StringValuation valuation = null, string encoding = Encodings.Utf8, string ns = NameSpaces.Json) : base(Types.String, encoding, ns)
    {
        Valuation = valuation;
    }
}