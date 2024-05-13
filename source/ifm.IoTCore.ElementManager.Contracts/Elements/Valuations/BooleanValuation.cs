namespace ifm.IoTCore.ElementManager.Contracts.Elements.Valuations;

using System.Collections.Generic;
using Common.Variant;

/// <summary>
/// Represents the valuation for a boolean type data element.
/// </summary>
public class BooleanValuation
{
    /// <summary>
    /// Gets the default value.
    /// </summary>
    [VariantProperty("default", IgnoredIfNull = true)]
    public bool? DefaultValue { get; set; }

    /// <summary>
    /// Gets the single values.
    /// </summary>
    [VariantProperty("single_values", IgnoredIfNull = true)]
    public List<SingleValue<bool>> SingleValues { get; set; }

    /// <summary>
    /// The parameterless constructor for the variant converter.
    /// </summary>
    [VariantConstructor]
    public BooleanValuation()
    {
    }

    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="defaultValue">The default value.</param>
    /// <param name="singleValues">The single values.</param>
    public BooleanValuation(bool? defaultValue = null, List<SingleValue<bool>> singleValues = null)
    {
        DefaultValue = defaultValue;
        SingleValues = singleValues;
    }
}