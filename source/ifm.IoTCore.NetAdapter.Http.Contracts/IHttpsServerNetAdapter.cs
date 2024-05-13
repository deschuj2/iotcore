namespace ifm.IoTCore.NetAdapter.Http.Contracts;

using System.Security.Cryptography.X509Certificates;
using NetAdapterManager.Contracts.Server;

public interface IHttpsServerNetAdapter : IServerNetAdapter
{
    string? CertificateThumbPrint { get; }
    X509Certificate2 Certificate { get; set; }
}