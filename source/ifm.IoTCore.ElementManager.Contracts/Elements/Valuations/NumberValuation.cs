namespace ifm.IoTCore.ElementManager.Contracts.Elements.Valuations;

using System.Collections.Generic;
using Common.Variant;

/// <summary>
/// Represents the valuation for a number type data element.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class NumberValuation<T> where T : struct
{
    /// <summary>
    /// Gets the minimum value.
    /// </summary>
    [VariantProperty("min", IgnoredIfNull = true)]
    public T? Min { get; set; }

    /// <summary>
    /// Gets the maximum value.
    /// </summary>
    [VariantProperty("max", IgnoredIfNull = true)]
    public T? Max { get; set; }

    /// <summary>
    /// Gets the default value.
    /// </summary>
    [VariantProperty("default", IgnoredIfNull = true)]
    public T? DefaultValue { get; set; }

    /// <summary>
    /// Gets the single values.
    /// </summary>
    [VariantProperty("single_values", IgnoredIfNull = true)]
    public List<SingleValue<T>> SingleValues { get; set; }

    /// <summary>
    /// Gets the value ranges.
    /// </summary>
    [VariantProperty("value_ranges", IgnoredIfNull = true)]
    public List<ValueRange<T>> ValueRanges { get; set; }

    /// <summary>
    /// Gets the gradient
    /// </summary>
    [VariantProperty("gradient", IgnoredIfNull = true)]
    public double? Gradient { get; set; }

    /// <summary>
    /// Gets the offset
    /// </summary>
    [VariantProperty("offset", IgnoredIfNull = true)]
    public double? Offset { get; set; }

    /// <summary>
    /// Gets the display format
    /// </summary>
    [VariantProperty("display_format", IgnoredIfNull = true)]
    public string DisplayFormat { get; set; }

    protected NumberValuation()
    {
    }

    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <param name="singleValues">The single values.</param>
    /// <param name="valueRanges">The value ranges.</param>
    /// <param name="gradient">The gradient.</param>
    /// <param name="offset">The offset.</param>
    /// <param name="displayFormat">The display format.</param>
    protected NumberValuation(T? min,
        T? max,
        T? defaultValue = null,
        List<SingleValue<T>> singleValues = null,
        List<ValueRange<T>> valueRanges = null,
        double? gradient = null,
        double? offset = null,
        string displayFormat = null)
    {
        Min = min;
        Max = max;
        DefaultValue = defaultValue;
        SingleValues = singleValues;
        ValueRanges = valueRanges;
        Gradient = gradient;
        Offset = offset;
        DisplayFormat = displayFormat;
    }
}