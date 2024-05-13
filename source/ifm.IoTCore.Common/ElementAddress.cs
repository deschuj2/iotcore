namespace ifm.IoTCore.Common;

using System;
using System.Text.RegularExpressions;

/// <summary>
/// Provides methods to create or process element addresses.
/// </summary>
public static class ElementAddress
{
    private static Regex IdentifierValidator => new(@"^[0-9a-zA-Z_\-\]\[]*$");

    /// <summary>
    /// The address separator character as character.
    /// </summary>
    public const char AddressSeparator = '/';

    /// <summary>
    /// Checks if an identifier is valid.
    /// </summary>
    /// <param name="identifier">The identifier to check.</param>
    /// <returns>true, if the identifier is valid; otherwise false.</returns>
    public static bool ValidateIdentifier(string identifier)
    {
        return !string.IsNullOrEmpty(identifier) && IdentifierValidator.IsMatch(identifier);
    }

    /// <summary>
    /// Replaces invalid characters in an identifier with the given character.
    /// </summary>
    /// <param name="identifier">The identifier to search.</param>
    /// <param name="replacement">The replacement string.</param>
    /// <returns>A new string with the replacements.</returns>
    public static string ReplaceInvalidCharacters(string identifier, string replacement = "_")
    {
        return Regex.Replace(identifier, @"[^0-9a-zA-Z_\-\]\[]", replacement, RegexOptions.None);
    }

    /// <summary>
    /// Creates a new address from the parent address and the identifier.
    /// </summary>
    /// <param name="parentAddress">The parent address.</param>
    /// <param name="identifier">The identifier.</param>
    /// <returns>The new address string.</returns>
    public static string Create(string parentAddress, string identifier)
    {
        return string.IsNullOrEmpty(parentAddress) ? identifier : $"{parentAddress}{AddressSeparator}{identifier}";
    }

    /// <summary>
    /// Creates a new address from the parent address and combination of the given identifiers.
    /// </summary>
    /// <param name="parentAddress">The parent address.</param>
    /// <param name="identifiers">The identifiers to combine.</param>
    /// <returns>The new address string.</returns>
    public static string Create(string parentAddress, params string[] identifiers)
    {
        return string.IsNullOrEmpty(parentAddress) ?
            string.Join(AddressSeparator.ToString(), identifiers) :
            $"{parentAddress}{AddressSeparator}{string.Join(AddressSeparator.ToString(), identifiers)}";
    }

    /// <summary>
    /// Checks if an address is valid.
    /// </summary>
    /// <param name="address">The address to check.</param>
    /// <returns>true if the address is valid; otherwise false.</returns>
    public static bool ValidateAddress(string address)
    {
        if (address == null) return false;
        if (address == string.Empty) return true;
        var tokens = address.Split(AddressSeparator);

        // Allow first token to be empty
        if (tokens[0] != string.Empty && !ValidateIdentifier(tokens[0])) return false;

        // Check other tokens
        for (var idx = 1; idx < tokens.Length; idx++)
        {
            if (!ValidateIdentifier(tokens[idx])) return false;
        }
        return true;
    }

    /// <summary>
    /// Patch address with device name.
    /// </summary>
    /// <param name="device">The device name.</param>
    /// <param name="address">The address to patch.</param>
    /// <returns>The patched address.</returns>
    public static string PatchAddress(string device, string address)
    {
        if (string.IsNullOrEmpty(address)) return device;
        return address[0] == AddressSeparator ? $"{device}{address}" : address;
    }

    /// <summary>
    /// Splits an address into its substrings.
    /// </summary>
    /// <param name="address">The address to split.</param>
    /// <returns>The array of substrings.</returns>
    public static string[] Split(string address)
    {
        return address.Split(AddressSeparator);
    }

    /// <summary>
    /// Patches the given element address and return the device name if provided.
    /// </summary>
    /// <param name="address">The given device/address string.</param>
    /// <param name="device">If provided the device name from the address; otherwise null.</param>
    /// <returns>The path for the element</returns>
    public static string SplitAddress(string address, out string device)
    {
        device = address.GetFirstToken(AddressSeparator);
        return address.RemoveFirstToken(AddressSeparator);
    }

    /// <summary>
    /// Gets the last identifier from an address.
    /// </summary>
    /// <param name="address">The address to search.</param>
    /// <returns>The last identifier from the address.</returns>
    public static string GetLastIdentifier(string address)
    {
        return address.GetLastToken(AddressSeparator, address.Length-1);
    }

    /// <summary>
    /// Gets the path from an address.
    /// </summary>
    /// <param name="address">The address to search.</param>
    /// <returns>The path from the address.</returns>
    public static string GetParentAddress(string address)
    {
        var pos = address.LastIndexOf(AddressSeparator);
        return pos != -1 ? address.Substring(0, pos) : address;
    }


    /// <summary>
    /// Checks is the provided address is a valid URI, where in IoTCore [] are allowed.
    /// </summary>
    /// <param name="address">The string to check.</param>
    /// <returns>true if the address is a valid URI; otherwise false.</returns>
    public static bool IsValidUri(string address)
    {
        var s = Regex.Replace(address, @"[\]\[]", "_", RegexOptions.None);
        return Uri.IsWellFormedUriString(s, UriKind.Absolute);
    }
}