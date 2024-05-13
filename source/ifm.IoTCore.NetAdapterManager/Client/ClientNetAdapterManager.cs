namespace ifm.IoTCore.NetAdapterManager.Client;

using System;
using System.Collections.Generic;
using System.Linq;
using Contracts.Client;

public class ClientNetAdapterManager : IClientNetAdapterManager
{
    private readonly List<IClientNetAdapterFactory> _clientNetAdapterFactories = new();

    public IEnumerable<IClientNetAdapterFactory> ClientNetAdapterFactories
    {
        get
        {
            lock (_clientNetAdapterFactories)
            {
                return _clientNetAdapterFactories.ToList();
            }
        }
    }

    public void RegisterClientNetAdapterFactory(IClientNetAdapterFactory clientNetAdapterFactory)
    {
        if (clientNetAdapterFactory == null) throw new ArgumentNullException(nameof(clientNetAdapterFactory));

        lock (_clientNetAdapterFactories)
        {
            if (_clientNetAdapterFactories.FirstOrDefault(x => x.Scheme == clientNetAdapterFactory.Scheme) == null)
            {
                _clientNetAdapterFactories.Add(clientNetAdapterFactory);
            }
        }
    }

    public void RemoveClientNetAdapterFactory(IClientNetAdapterFactory clientNetAdapterFactory)
    {
        if (clientNetAdapterFactory == null) throw new ArgumentNullException(nameof(clientNetAdapterFactory));

        lock (_clientNetAdapterFactories)
        {
            _clientNetAdapterFactories.Remove(clientNetAdapterFactory);
            clientNetAdapterFactory.Dispose();
        }
    }

    public IClientNetAdapter CreateClientNetAdapter(Uri targetUri)
    {
        if (targetUri == null) throw new ArgumentNullException(nameof(targetUri));

        lock (_clientNetAdapterFactories)
        {
            var clientFactory = _clientNetAdapterFactories.FirstOrDefault(x => x.Scheme == targetUri.Scheme);
            if (clientFactory == null)
            {
                throw new Exception($"Client factory not found for {targetUri.Scheme}");
            }
            return clientFactory.CreateClient(targetUri);
        }
    }

    public void Dispose()
    {
        lock (_clientNetAdapterFactories)
        {
            foreach (var item in _clientNetAdapterFactories)
            {
                item.Dispose();
            }
            _clientNetAdapterFactories.Clear();
        }
    }
}