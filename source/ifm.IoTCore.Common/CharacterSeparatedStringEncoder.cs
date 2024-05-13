namespace ifm.IoTCore.Common;

using System;
using System.Text;

/// <summary>
/// Provides methods to convert between arrays and character separated string.
/// </summary>
public class CharacterSeparatedStringEncoder
{
    /// <summary>
    /// Converts a byte array to a character separated string.
    /// </summary>
    /// <param name="items">The items to convert.</param>
    /// <param name="separator">The separator character.</param>
    /// <returns>The converted string.</returns>
    public static string ByteArrayToCharacterSeparatedString(byte[] items, char separator = ',')
    {
        if (items == null) throw new ArgumentNullException();
        if (items.Length == 0) return string.Empty;

        var sb = new StringBuilder();
        sb.AppendFormat("{0}", items[0]);
        for (var i = 1; i < items.Length; i++)
        {
            sb.AppendFormat($"{separator}{{0:d2}}", items[i]);
        }
        return sb.ToString();
    }

    /// <summary>
    /// Converts a character separated string to a byte array.
    /// </summary>
    /// <param name="str">The string to convert.</param>
    /// <param name="separator">The separator character.</param>
    /// <returns>The converted array.</returns>
    public static byte[] CharacterSeparatedStringToByteArray(string str, char separator = ',')
    {
        if (str == null) throw new ArgumentNullException();

        var tokens = str.Split(separator);
        var items = new byte[tokens.Length];
        for (var i = 0; i < tokens.Length; i++)
        {
            items[i] = Convert.ToByte(tokens[i]);
        }
        return items;
    }

    /// <summary>
    /// Converts an integer array to a character separated string.
    /// </summary>
    /// <param name="items">The items to convert.</param>
    /// <param name="separator">The separator character.</param>
    /// <returns>The converted string.</returns>
    public static string IntegerArrayToCharacterSeparatedString(int[] items, char separator = ',')
    {
        if (items == null) throw new ArgumentNullException();
        if (items.Length == 0) return string.Empty;

        var sb = new StringBuilder();
        sb.AppendFormat("{0}", items[0]);
        for (var i = 1; i < items.Length; i++)
        {
            sb.AppendFormat($"{separator}{{0}}", items[i]);
        }
        return sb.ToString();
    }

    /// <summary>
    /// Converts a character separated string to an integer array.
    /// </summary>
    /// <param name="str">The string to convert.</param>
    /// <param name="separator">The separator character.</param>
    /// <returns>The converted array.</returns>
    public static int[] CharacterSeparatedStringToIntegerArray(string str, char separator = ',')
    {
        if (str == null) throw new ArgumentNullException();

        var tokens = str.Split(separator);
        var items = new int[tokens.Length];
        for (var i = 0; i < tokens.Length; i++)
        {
            items[i] = Convert.ToByte(tokens[i]);
        }
        return items;
    }
}