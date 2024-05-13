namespace ifm.IoTCore.Common.Exceptions;

/// <summary>
/// The exception that is thrown when data provided with a request is invalid (Response code: 422).
/// </summary>
public class DataInvalidException : IoTCoreException
{
    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="detailsMessage">The details what caused the error or how to fix it.</param>
    public DataInvalidException(string message = "Data invalid", string detailsMessage = null) : base(ResponseCodes.DataInvalid, message, detailsMessage)
    {
    }
}
