namespace ifm.IoTCore.ElementManager.Contracts.Elements.Valuations;

using System.Collections.Generic;
using Common.Variant;

/// <summary>
/// Represents the valuation for a float type data element.
/// </summary>
public class FloatValuation : NumberValuation<float>
{
    /// <summary>
    /// Gets the number of decimals.
    /// </summary>
    [VariantProperty("decimalplaces", IgnoredIfNull = true)]
    public int? Decimals { get; set; }

    /// <summary>
    /// The parameterless constructor for the variant converter.
    /// </summary>
    [VariantConstructor]
    public FloatValuation()
    {
    }

    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <param name="decimals">The number of decimals.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <param name="singleValues">The single values.</param>
    /// <param name="valueRanges">The value ranges.</param>
    /// <param name="gradient">The gradient.</param>
    /// <param name="offset">The offset.</param>
    /// <param name="displayFormat">The display format.</param>
    public FloatValuation(float? min,
        float? max,
        int? decimals,
        float? defaultValue = null,
        List<SingleValue<float>> singleValues = null,
        List<ValueRange<float>> valueRanges = null,
        double? gradient = null,
        double? offset = null,
        string displayFormat = null) : base(min, max, defaultValue, singleValues, valueRanges, gradient, offset, displayFormat)
    {
        Decimals = decimals;
    }
}

/// <summary>
/// Represents the valuation for a double type data element.
/// </summary>
public class DoubleValuation : NumberValuation<double>
{
    /// <summary>
    /// Gets the number of decimals.
    /// </summary>
    [VariantProperty("decimalplaces", IgnoredIfNull = true)]
    public int? Decimals { get; set; }

    /// <summary>
    /// The parameterless constructor for the variant converter.
    /// </summary>
    [VariantConstructor]
    public DoubleValuation()
    {
    }

    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <param name="decimals">The number of decimals.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <param name="singleValues">The single values.</param>
    /// <param name="valueRanges">The value ranges.</param>
    /// <param name="gradient">The gradient.</param>
    /// <param name="offset">The offset.</param>
    /// <param name="displayFormat">The display format.</param>
    public DoubleValuation(double? min,
        double? max,
        int? decimals,
        double? defaultValue = null,
        List<SingleValue<double>> singleValues = null,
        List<ValueRange<double>> valueRanges = null,
        double? gradient = null,
        double? offset = null,
        string displayFormat = null) : base(min, max, defaultValue, singleValues, valueRanges, gradient, offset, displayFormat)
    {
        Decimals = decimals;
    }
}