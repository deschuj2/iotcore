namespace Sample04;

using System;
using ifm.IoTCore.Factory;

internal class Program
{
    static void Main()
    {
        try
        {
            var ioTCore = IoTCoreFactory.Create("MyIoTCore");

            var struct1 = ioTCore.ElementManager.CreateStructureElement(ioTCore.Root, 
                "struct1");

            var event1 = ioTCore.ElementManager.CreateEventElement(struct1, 
                "event1");
            event1.EventRaised += (sender, args) =>
            {
                Console.WriteLine("event1 raised");
            }; 
            event1.Raise();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        Console.ReadLine();
    }
}