namespace ifm.IoTCore.DataStore.Contracts;

/// <summary>
/// Provides functionality to interact with a data store.
/// </summary>
public interface IDataStore
{
    /// <summary>
    /// Reads the configuration depending on sectionKey and configKey.
    /// </summary>
    /// <typeparam name="T">The type of the configuration object.</typeparam>
    /// <param name="sectionKey">The specific section key.</param>
    /// <param name="configKey">The specific config key.</param>
    /// <returns>The found config depending on sectionKey and configKey.</returns>
    T Get<T>(string sectionKey, string configKey);

    /// <summary>
    /// Overwrites or updates the configuration depending on sectionKey and configKey.
    /// </summary>
    /// <typeparam name="T">The type of the configuration object.</typeparam>
    /// <param name="sectionKey">The specific section key.</param>
    /// <param name="configKey">The specific config key.</param>
    /// <param name="config">The config that have to be stored.</param>
    void Set<T>(string sectionKey, string configKey, T config);
}