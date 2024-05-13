namespace ifm.IoTCore.Common.Exceptions;

/// <summary>
/// The exception that is thrown when an element already exists (Response code: 901).
/// </summary>
public class AlreadyExistsException : IoTCoreException
{
    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="detailsMessage">The details what caused the error or how to fix it.</param>
    public AlreadyExistsException(string message = "Element already exists", string detailsMessage = null) : base(ResponseCodes.AlreadyExists, message, detailsMessage)
    {
    }
}
