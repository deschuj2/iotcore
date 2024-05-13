namespace ifm.IoTCore.Common.Exceptions;

/// <summary>
/// The exception that is thrown when an element can't be accessed, because it is locked (Response code: 423).
/// </summary>
public class LockedException : IoTCoreException
{
    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="detailsMessage">The details what caused the error or how to fix it.</param>
    public LockedException(string message = "Element locked", string detailsMessage = null) : base(ResponseCodes.Locked, message, detailsMessage)
    {
    }
}