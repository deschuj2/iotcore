namespace Sample16;

using System;
using ifm.IoTCore.Common;
using ifm.IoTCore.ElementManager.Contracts.Elements;
using ifm.IoTCore.Factory;

internal class Program
{
    static void Main()
    {
        try
        {
            var ioTCore = IoTCoreFactory.Create("myiotcore");

            var data1 = ioTCore.ElementManager.CreateSimpleDataElement(ioTCore.Root, 
                "data1", 
                true,
                true,
                true,
                100);
            var event1 = (IEventElement)data1.GetElementByIdentifier(Identifiers.DataChanged);
            event1.EventRaised += (_, _) =>
            {
                Console.WriteLine("event1 raised");
            };

            data1.Value = 200;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        Console.ReadLine();
    }
}