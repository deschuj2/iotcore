namespace ifm.IoTCore.Common;

/// <summary>
/// Provides extension methods for the hash code type.
/// The hash code type is available in .NET Standard 2.1.
/// Until the project is upgraded to .NET Standard 2.1 use this method and integer values.
/// </summary>
public static class HashCodeExtensions
{
    /// <summary>
    /// Combines two values into a hash code.
    /// </summary>
    /// <param name="value1">The first value to combine into the hash code.</param>
    /// <param name="value2">The second value to combine into the hash code.</param>
    /// <returns>The hash code that represents the two values.</returns>
    public static int CombineHashCodes(int value1, int value2)
    {
        return ((value1 << 5) + value1) ^ value2;
    }
}