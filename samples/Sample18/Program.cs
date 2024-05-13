namespace Sample18;

using System;
using System.Threading;
using ifm.IoTCore.ElementManager.Contracts.Elements;
using ifm.IoTCore.Factory;

internal class Program
{
    private static Timer _eventTimer;

    static void Main()
    {
        try
        {
            var ioTCore = IoTCoreFactory.Create("MyIoTCore");

            var event1 = ioTCore.ElementManager.CreateEventElement(ioTCore.Root, 
                "event1");
            event1.EventRaised += HandleEvent1;

            _eventTimer = new Timer(HandleTimer, event1, 5000, 3000);

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        Console.ReadLine();

        _eventTimer.Dispose();
    }

    private static void HandleTimer(object state)
    {
        ((IEventElement)state).Raise();
    }

    private static void HandleEvent1(object sender, EventArgs _)
    {
        Console.WriteLine($"Received event from {((IEventElement)sender).Address}");
    }
}