namespace ifm.IoTCore.Common;

using System;

/// <summary>
/// Provides extension methods for floating point number types.
/// </summary>
public static class FloatingPointNumberExtensions
{
    /// <summary>
    /// Compares two float values with precision.
    /// </summary>
    /// <param name="thisValue">The value to compare.</param>
    /// <param name="otherValue">The value to compare against.</param>
    /// <param name="precision">The precision of the comparison. If the difference between the values is less than the precision, the values are considered equal.</param>
    /// <returns>true if the difference between the values is within the precision; otherwise false.</returns>
    public static bool EqualsWithPrecision(this float thisValue, float otherValue, float precision = 0.001f)
    {
        return Math.Abs(thisValue - otherValue) < precision;
    }

    /// <summary>
    /// Compares two double values with precision.
    /// </summary>
    /// <param name="thisValue">The value to compare.</param>
    /// <param name="otherValue">The value to compare against.</param>
    /// <param name="precision">The precision of the comparison. If the difference between the values is less than the precision, the values are considered equal.</param>
    /// <returns>true if the difference between the values is within the precision; otherwise false.</returns>
    public static bool EqualsWithPrecision(this double thisValue, double otherValue, double precision = 0.001)
    {
        return Math.Abs(thisValue - otherValue) < precision;
    }

    /// <summary>
    /// Compares two decimal values with precision.
    /// </summary>
    /// <param name="thisValue">The value to compare.</param>
    /// <param name="otherValue">The value to compare against.</param>
    /// <param name="precision">The precision of the comparison. If the difference between the values is less than the precision, the values are considered equal.</param>
    /// <returns>true if the difference between the values is within the precision; otherwise false.</returns>
    public static bool EqualsWithPrecision(this decimal thisValue, decimal otherValue, decimal precision = 0.001m)
    {
        return Math.Abs(thisValue - otherValue) < precision;
    }
}