namespace Sample19;

using System;
using ifm.IoTCore.Factory;
using ifm.IoTCore.MessageConverter.Json;
using ifm.IoTCore.NetAdapter.Http;

internal class Program
{
    private static void Main()
    {
        var ioTCore = IoTCoreFactory.Create("MyIoTCore");

        HttpServerNetAdapter httpServer = null;
        try
        {
            httpServer = new HttpServerNetAdapter(ioTCore.MessageHandler, 
                new Uri("http://127.0.0.1:8000"),
                new MessageConverter());

            ioTCore.ServerNetAdapterManager.RegisterServerNetAdapter(httpServer);

            httpServer.Start();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        Console.ReadLine();
        httpServer?.Stop();
        ioTCore.Dispose();
    }
}