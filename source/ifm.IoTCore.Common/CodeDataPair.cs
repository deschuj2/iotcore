namespace ifm.IoTCore.Common;

using Variant;

/// <summary>
/// Represents a code data pair.
/// </summary>
public class CodeDataPair
{
    /// <summary>
    /// The code of the query.
    /// </summary>
    [VariantProperty("code", Required = true)]
    public int Code { get; set; }

    /// <summary>
    /// The value of the element.
    /// </summary>
    [VariantProperty("data", IgnoredIfNull = true)]
    public Variant.Variant Data { get; set; }

    /// <summary>
    /// The parameterless constructor for the variant converter.
    /// </summary>
    [VariantConstructor]
    public CodeDataPair()
    {
    }

    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="code">The code of the query.</param>
    /// <param name="data">The data of the element.</param>
    public CodeDataPair(int code, Variant.Variant data)
    {
        Code = code;
        Data = data;
    }
}