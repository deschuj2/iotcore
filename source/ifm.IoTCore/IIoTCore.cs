namespace ifm.IoTCore;

using System;
using System.Collections.Generic;
using ElementManager.Contracts;
using ElementManager.Contracts.Elements;
using EventSender.Contracts;
using Logger.Contracts;
using MessageHandler.Contracts;
using NetAdapterManager.Contracts.Client;
using NetAdapterManager.Contracts.Server;
using PersistenceManager.Contracts;
using ServiceData.Responses;
using UserManager.Contracts;

/// <summary>
/// Provides functionality to interact with the IoTCore.
/// </summary>
public interface IIoTCore : IDisposable
{
    /// <summary>
    /// Gets the identifier of the root element.
    /// </summary>
    string Identifier { get; }

    /// <summary>
    /// Gets the unique identifier of the root element.
    /// </summary>
    string Uid { get; }

    /// <summary>
    /// Gets the API version.
    /// </summary>
    string ApiVersion { get; }

    /// <summary>
    /// Gets the list of catalogs.
    /// </summary>
    IEnumerable<CatalogInfo> Catalogs { get; }

    /// <summary>
    /// Adds a catalog to the list of catalogs
    /// </summary>
    /// <param name="catalogInfo"></param>
    void AddCatalog(CatalogInfo catalogInfo);

    /// <summary>
    /// Gets the list of components.
    /// </summary>
    IEnumerable<ComponentInfo> Components { get; }

    /// <summary>
    /// Adds a component to the list of components.
    /// </summary>
    /// <param name="componentInfo"></param>
    void AddComponent(ComponentInfo componentInfo);

    /// <summary>
    /// Gets the list of device classes.
    /// </summary>
    IEnumerable<string> DeviceClass { get; }

    /// <summary>
    /// Gets the root element.
    /// </summary>
    IBaseElement Root { get; }

    /// <summary>
    /// Gets the user manager.
    /// </summary>
    IUserManager UserManager { get; }

    /// <summary>
    /// Gets the element manager.
    /// </summary>
    IElementManager ElementManager { get; }

    /// <summary>
    /// Gets the message handler.
    /// </summary>
    IMessageHandler MessageHandler { get; }

    /// <summary>
    /// Gets the client network adapter manager.
    /// </summary>
    IClientNetAdapterManager ClientNetAdapterManager { get; }

    /// <summary>
    /// Gets the server network adapter manager.
    /// </summary>
    IServerNetAdapterManager ServerNetAdapterManager { get; }

    /// <summary>
    /// Gets the event sender
    /// </summary>
    IEventSender EventSender { get; }

    /// <summary>
    /// Gets the persistence manager.
    /// </summary>
    IPersistenceManager PersistenceManager { get; }

    /// <summary>
    /// Gets the logger.
    /// </summary>
    ILogger Logger { get; }
}