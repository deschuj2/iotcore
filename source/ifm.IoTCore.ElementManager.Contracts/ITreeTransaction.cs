namespace ifm.IoTCore.ElementManager.Contracts;

/// <summary>
/// Provides functionality to interact with an element tree transaction.
/// </summary>
public interface ITreeTransaction
{
    /// <summary>
    /// Begins an element tree transaction.
    /// </summary>
    void Begin();

    /// <summary>
    /// Ends an element tree transaction.
    /// </summary>
    void End();
}