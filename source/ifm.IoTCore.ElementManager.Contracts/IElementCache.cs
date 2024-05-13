namespace ifm.IoTCore.ElementManager.Contracts;

using Elements;

/// <summary>
/// Provides functionality to interact with the element cache.
/// </summary>
public interface IElementCache
{
    /// <summary>
    /// Gets the element with the specified address.
    /// </summary>
    /// <param name="address">The address of the element to get.</param>
    /// <returns>The element with the specified address if the element is found; otherwise null.</returns>
    IBaseElement GetElementByAddress(string address);
}
