namespace ifm.IoTCore.NetAdapterManager.Server;

using System;
using System.Collections.Generic;
using System.Linq;
using Contracts.Server;
using Logger.Contracts;

public class ServerNetAdapterManager : IServerNetAdapterManager
{
    private readonly ILogger _logger;
    private readonly List<IServerNetAdapter> _serverNetAdapters = new();

    public ServerNetAdapterManager(ILogger logger)
    {
        _logger = logger;
    }

    public IEnumerable<IServerNetAdapter> ServerNetAdapters
    {
        get
        {
            lock (_serverNetAdapters)
            {
                return _serverNetAdapters.ToList();
            }
        }
    }

    public void RegisterServerNetAdapter(IServerNetAdapter serverNetAdapter)
    {
        if (serverNetAdapter == null) throw new ArgumentNullException(nameof(serverNetAdapter));

        lock (_serverNetAdapters)
        {
            _serverNetAdapters.Add(serverNetAdapter);
        }
    }

    public void RemoveServerNetAdapter(IServerNetAdapter serverNetAdapter)
    {
        if (serverNetAdapter == null) throw new ArgumentNullException(nameof(serverNetAdapter));

        lock (_serverNetAdapters)
        {
            _serverNetAdapters.Remove(serverNetAdapter);
            serverNetAdapter.Stop();
            serverNetAdapter.Dispose();
        }
    }

    public IServerNetAdapter FindServerNetAdapter(Uri uri)
    {
        lock (_serverNetAdapters)
        {
            return _serverNetAdapters.FirstOrDefault(x => x.Uri == uri);
        }
    }

    public IEnumerable<IServerNetAdapter> FindServerNetAdapters(string scheme)
    {
        lock (_serverNetAdapters)
        {
            return _serverNetAdapters.Where(x => x.Scheme == scheme);
        }
    }

    public void Dispose()
    {
        lock (_serverNetAdapters)
        {
            foreach (var item in _serverNetAdapters)
            {
                try
                {
                    item?.Stop();
                }
                catch (Exception e)
                {
                    _logger?.Error($"ServerNetAdapterManager.Dispose(). Error stopping an servernetadapter: '{e.Message}'.");
                }

                try
                {
                    item?.Dispose();
                }
                catch (Exception e)
                {
                    _logger?.Error($"ServerNetAdapterManager.Dispose(). Error disposing an servernetadapter: '{e.Message}'.");
                }
            }

            _serverNetAdapters.Clear();
        }
    }
}