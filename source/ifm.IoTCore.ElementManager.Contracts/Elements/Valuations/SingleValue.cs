namespace ifm.IoTCore.ElementManager.Contracts.Elements.Valuations;

using Common.Variant;

/// <summary>
/// Represents a single value.
/// </summary>
public class SingleValue<T>
{
    /// <summary>
    /// Gets the value.
    /// </summary>
    [VariantProperty("value", Required = true)]
    public T Value { get; set; }

    /// <summary>
    /// Gets the value text.
    /// </summary>
    [VariantProperty("text", Required = true)]
    public string Text { get; set; }

    /// <summary>
    /// The parameterless constructor for the variant converter.
    /// </summary>
    [VariantConstructor]
    public SingleValue()
    {
    }

    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="text">The value text.</param>
    public SingleValue(T value, string text)
    {
        Value = value;
        Text = text;
    }
}

