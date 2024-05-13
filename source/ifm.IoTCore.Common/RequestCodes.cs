﻿namespace ifm.IoTCore.Common;

/// <summary>
/// Specifies the request codes for the request messages.
/// </summary>
public class RequestCodes
{
    /// <summary>
    /// A request that requires a response (10).
    /// </summary>
    public const int Request = 10;

    /// <summary>
    /// A notification event that does not require a response (80).
    /// </summary>
    public const int Event = 80;

    /// <summary>
    /// Checks if the given code is a request or an event code.
    /// </summary>
    /// <param name="code">The code to check.</param>
    /// <returns>true if the code is a request or event code.; otherwise false.</returns>
    public static bool IsRequestOrEvent(int code)
    {
        return code is Request or Event;
    }
}