namespace ifm.IoTCore.UserManager.Contracts;

/// <summary>
/// Provides functionality to interact with the user manager.
/// </summary>
public interface IUserManager
{
    /// <summary>
    /// Gets the active authentication scheme.
    /// </summary>
    string Scheme { get; }

    /// <summary>
    /// Gets if authentication is required.
    /// </summary>
    bool IsAuthenticationRequired { get; }

    /// <summary>
    /// Authenticates the user authentication information.
    /// </summary>
    /// <param name="user">The user name in base64 encoding.</param>
    /// <param name="password">The password in base64 encoding.</param>
    /// <returns>true, if a user with name and password exists; otherwise false.</returns>
    bool Authenticate(string user, string password);
}