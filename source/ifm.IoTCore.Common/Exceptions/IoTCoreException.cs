namespace ifm.IoTCore.Common.Exceptions;

using System;

/// <summary>
/// Represents errors that occur during application execution.
/// </summary>
public class IoTCoreException : Exception
{
    /// <summary>
    /// Gets the response code.
    /// </summary>
    public int ResponseCode { get; }

    /// <summary>
    /// Gets the error code.
    /// </summary>
    public int? ErrorCode { get; }

    /// <summary>
    /// Gets the error details what caused the error or how to fix the problem.
    /// </summary>
    public string ErrorDetails { get; }

    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="responseCode">The response code.</param>
    public IoTCoreException(int responseCode)
    {
        ResponseCode = responseCode;
    }

    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="responseCode">The response code.</param>
    /// <param name="errorMessage">The error message.</param>
    public IoTCoreException(int responseCode,
        string errorMessage) : base(errorMessage)
    {
        ResponseCode = responseCode;
    }

    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="responseCode">The response code.</param>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="errorCode">The error code.</param>
    public IoTCoreException(int responseCode,
        string errorMessage,
        int errorCode) : base(errorMessage)
    {
        ResponseCode = responseCode;
        ErrorCode = errorCode;
    }

    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="responseCode">The response code.</param>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="errorDetails">The details what caused the error or how to fix it.</param>
    public IoTCoreException(int responseCode,
        string errorMessage,
        string errorDetails) : base(errorMessage)
    {
        ResponseCode = responseCode;
        ErrorDetails = errorDetails;
    }
    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="responseCode">The response code.</param>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="errorCode">The error code.</param>
    /// <param name="errorDetails">The details what caused the error or how to fix it.</param>
    public IoTCoreException(int responseCode,
        string errorMessage,
        int errorCode,
        string errorDetails) : base(errorMessage)
    {
        ResponseCode = responseCode;
        ErrorCode = errorCode;
        ErrorDetails = errorDetails;
    }
}