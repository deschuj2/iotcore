namespace ifm.IoTCore.Factory;

using System;
using Common;
using ElementManager;
using EventSender;
using Logger.Contracts;
using MessageHandler;
using NetAdapterManager.Client;
using NetAdapterManager.Server;
using PersistenceManager;
using UserManager;

/// <summary>
/// Exposes static methods for creating a new instance of the IoTCore class.
/// </summary>
public class IoTCoreFactory
{
    /// <summary>
    /// Creates a new instance of the class.
    /// </summary>
    /// <param name="identifier">Specifies the identifier for the root device element.</param>
    /// <param name="logger">The logger used in the IoTCore modules.</param>
    /// <returns>The newly created instance.</returns>
    public static IIoTCore Create(string identifier, ILogger logger = null)
    {
        if (string.IsNullOrEmpty(identifier)) throw new ArgumentNullException(nameof(identifier));
        if (!ElementAddress.ValidateIdentifier(identifier)) throw new ArgumentException(nameof(identifier));

        var userManager = new UserManager();
        var clientNetAdapterManager = new ClientNetAdapterManager();
        var serverNetAdapterManager = new ServerNetAdapterManager(logger);
        var elementCache = new ElementCache();
        var persistenceManager = new PersistenceManager(identifier, elementCache, logger);
        var eventSender = new EventSender(elementCache, clientNetAdapterManager, serverNetAdapterManager, logger);
        var messageHandler = new MessageHandler(identifier, elementCache, userManager, logger);
        var elementManager = new ElementManager(elementCache, persistenceManager, eventSender);

        var ioTCore = new IoTCore(identifier,
            userManager,
            elementManager,
            messageHandler,
            clientNetAdapterManager,
            serverNetAdapterManager,
            eventSender,
            persistenceManager,
            logger);

        return ioTCore;
    }
}