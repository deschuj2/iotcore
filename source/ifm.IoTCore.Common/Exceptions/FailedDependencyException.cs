namespace ifm.IoTCore.Common.Exceptions;

/// <summary>
/// The exception that is thrown when a failed dependency is detected (Response code: 424).
/// </summary>
public class FailedDependencyException : IoTCoreException
{
    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="detailsMessage">The details what caused the error or how to fix it.</param>
    public FailedDependencyException(string message = "Failed dependency", string detailsMessage = null) : base(ResponseCodes.FailedDependency, message, detailsMessage)
    {
    }
}