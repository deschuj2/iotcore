namespace ifm.IoTCore.Common.Exceptions;

/// <summary>
/// The exception that is thrown when a service failed (Response code: 550).
/// </summary>
public class ServiceFailedException : IoTCoreException
{
    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="detailsMessage">The details what caused the error or how to fix it.</param>
    public ServiceFailedException(string message = "Service failed", string detailsMessage = null) : base(ResponseCodes.ServiceFailed, message, detailsMessage)
    {
    }
}