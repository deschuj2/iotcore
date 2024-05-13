namespace ifm.IoTCore.PersistenceManager.Contracts;

using System.Collections.Generic;
using Common.Variant;

/// <summary>
/// Provides functionality to interact with the persistence manager.
/// </summary>
public interface IPersistenceManager
{
    /// <summary>
    /// Represents a persisted service call.
    /// </summary>
    public class ServiceCallInfo
    {
        /// <summary>
        /// Creates a new instance of the class.
        /// </summary>
        /// <param name="id">The id which identifies the item.</param>
        /// <param name="serviceAddress">The address of the called service.</param>
        /// <param name="serviceData">The incoming data of the called service.</param>
        /// <param name="cid">The context id</param>
        public ServiceCallInfo(int id, string serviceAddress, Variant serviceData, int? cid)
        {
            Id = id;
            ServiceAddress = serviceAddress;
            ServiceData = serviceData;
            Cid = cid;
        }

        /// <summary>
        /// The id which identifies the item.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// The address of the called service.
        /// </summary>
        public string ServiceAddress { get; }

        /// <summary>
        /// The incoming data of the called service.
        /// </summary>
        public Variant ServiceData { get; }

        /// <summary>
        /// The context id of the service call.
        /// </summary>
        public int? Cid { get; }
    }

    /// <summary>
    /// Gets the persisted service calls.
    /// </summary>
    IEnumerable<ServiceCallInfo> PersistedItems { get; }

    /// <summary>
    /// Persists a service call.
    /// </summary>
    /// <param name="serviceAddress">The address of the called service.</param>
    /// <param name="serviceData">The incoming data of the called service.</param>
    /// <param name="cid">The context id of the service call.</param>
    /// <returns>The id which identifies the item.</returns>
    int Persist(string serviceAddress, Variant serviceData, int? cid);

    /// <summary>
    /// Removes a persisted service call.
    /// </summary>
    /// <param name="id">The id which identifies the item.</param>
    void Remove(int id);

    /// <summary>
    /// Reruns the persisted service calls against the IoTCore.
    /// </summary>
    void Restore();
}