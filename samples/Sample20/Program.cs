namespace Sample20;

using System;
using ifm.IoTCore.Factory;
using ifm.IoTCore.MessageConverter.Json;
using ifm.IoTCore.NetAdapter.Http;

internal class Program
{
    static void Main()
    {
        var ioTCore = IoTCoreFactory.Create("MyIoTCore");
        try
        {
            ioTCore.ClientNetAdapterManager.RegisterClientNetAdapterFactory(
                new HttpClientNetAdapterFactory(new MessageConverter()));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        Console.ReadLine();
        ioTCore.Dispose();
    }
}