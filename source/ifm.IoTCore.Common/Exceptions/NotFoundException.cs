namespace ifm.IoTCore.Common.Exceptions;

/// <summary>
/// The exception that is thrown when an element can't be found (Response code: 404).
/// </summary>
public class NotFoundException : IoTCoreException
{
    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="detailsMessage">The details what caused the error or how to fix it.</param>
    public NotFoundException(string message = "Not found", string detailsMessage = null) : base(ResponseCodes.NotFound, message, detailsMessage)
    {
    }
}