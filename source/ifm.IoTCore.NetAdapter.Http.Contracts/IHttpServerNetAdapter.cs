namespace ifm.IoTCore.NetAdapter.Http.Contracts
{
    using NetAdapterManager.Contracts.Server;

    public interface IHttpServerNetAdapter : IServerNetAdapter
    {
        public AllowedServicesMode AllowedServicesMode { get; set; }
    }
}
