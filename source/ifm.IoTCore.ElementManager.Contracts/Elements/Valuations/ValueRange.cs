namespace ifm.IoTCore.ElementManager.Contracts.Elements.Valuations;

using Common.Variant;

/// <summary>
/// Represents a value range.
/// </summary>
public class ValueRange<T>
{
    /// <summary>
    /// Gets the lower value.
    /// </summary>
    [VariantProperty("lower_value", Required = true)]
    public T LowerValue { get; set; }

    /// <summary>
    /// Gets the upper value.
    /// </summary>
    [VariantProperty("upper_value", Required = true)]
    public T UpperValue { get; set; }

    /// <summary>
    /// Gets the value range text.
    /// </summary>
    [VariantProperty("text", Required = true)]
    public string Text { get; set; }

    /// <summary>
    /// The parameterless constructor for the variant converter.
    /// </summary>
    [VariantConstructor]
    public ValueRange()
    {
    }

    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="lowerValue">The lower value.</param>
    /// <param name="upperValue">The upper value.</param>
    /// <param name="text">The value range text.</param>
    public ValueRange(T lowerValue, T upperValue, string text)
    {
        LowerValue = lowerValue;
        UpperValue = upperValue;
        Text = text;
    }
}

