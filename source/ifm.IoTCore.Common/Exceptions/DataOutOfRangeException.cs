namespace ifm.IoTCore.Common.Exceptions;

/// <summary>
/// The exception that is thrown when data provided with a request is out of range (Response code: 416).
/// </summary>
public class DataOutOfRangeException : IoTCoreException
{
    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="detailsMessage">The details what caused the error or how to fix it.</param>
    public DataOutOfRangeException(string message = "Data is out of range", string detailsMessage = null) : base(ResponseCodes.DataOutOfRange, message, detailsMessage)
    {
    }
}