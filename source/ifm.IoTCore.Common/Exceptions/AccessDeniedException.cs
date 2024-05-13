namespace ifm.IoTCore.Common.Exceptions;

/// <summary>
/// The exception that is thrown when access to an element is denied (Response code: 401).
/// </summary>
public class AccessDeniedException : IoTCoreException
{
    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="detailsMessage">The details what caused the error or how to fix it.</param>
    public AccessDeniedException(string message = "Access denied", string detailsMessage = null) : base(ResponseCodes.AccessDenied, message, detailsMessage)
    {
    }
}