namespace ifm.IoTCore.Common.Exceptions;

/// <summary>
/// The exception that is thrown when access to a resource timed out (Response code: 504).
/// </summary>
public class TimeoutException : IoTCoreException
{
    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="detailsMessage">The details what caused the error or how to fix it.</param>
    public TimeoutException(string message = "Timeout", string detailsMessage = null) : base(ResponseCodes.Timeout, message, detailsMessage)
    {
    }
}