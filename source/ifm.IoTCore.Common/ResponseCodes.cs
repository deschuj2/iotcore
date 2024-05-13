namespace ifm.IoTCore.Common;

/// <summary>
/// Specifies well-known error codes.
/// </summary>
public class ResponseCodes
{
    /// <summary>
    /// The service execution is successful (200).
    /// </summary>
    public const int Success = 200;

    /// <summary>
    /// The service execution is successful and a reboot is required (230).
    /// </summary>
    public const int SuccessReboot = 230;

    /// <summary>
    /// The service request is invalid or malformed (400).
    /// </summary>
    public const int BadRequest = 400;

    /// <summary>
    /// The access to a service is denied (401).
    /// </summary>
    public const int AccessDenied = 401;

    /// <summary>
    /// The requested service does not exist (404).
    /// </summary>
    public const int NotFound = 404;

    /// <summary>
    /// The provided service data is out of range (416).
    /// </summary>
    public const int DataOutOfRange = 416;

    /// <summary>
    /// The provided service data is invalid (422).
    /// </summary>
    public const int DataInvalid = 422;

    /// <summary>
    /// The service is locked, disabled or busy at the moment (423).
    /// </summary>
    public const int Locked = 423;

    /// <summary>
    /// A dependency is missing to execute the service (424).
    /// </summary>
    public const int FailedDependency = 424;

    /// <summary>
    /// Internal error (500).
    /// </summary>
    public const int InternalError = 500;

    /// <summary>
    /// The service is not implemented (501).
    /// </summary>
    public const int NotImplemented = 501;

    /// <summary>
    /// The service is currently not available (503).
    /// </summary>
    public const int NotAvailable = 503;

    /// <summary>
    /// A call to a remote service timeout (504).
    /// </summary>
    public const int Timeout = 504;

    /// <summary>
    /// The memory / storage of the device is insufficient (507).
    /// </summary>
    public const int InsufficientStorage = 507;

    /// <summary>
    /// The service execution failed (550).
    /// </summary>
    public const int ServiceFailed = 550;

    /// <summary>
    /// The element already exists (901).
    /// </summary>
    public const int AlreadyExists = 901;

    /// <summary>
    /// Checks if the given code is a success code.
    /// </summary>
    /// <param name="code">The code to check.</param>
    /// <returns>true if the code is a success code; otherwise false.</returns>
    public static bool IsSuccess(int code)
    {
        return code is >= 200 and < 300;
    }
}