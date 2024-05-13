namespace Sample01;

using System;
using ifm.IoTCore.Factory;

internal class Program
{
    static void Main()
    {
        try
        {
            var ioTCore = IoTCoreFactory.Create("MyIoTCore");
            Console.WriteLine(ioTCore.ApiVersion);
            Console.WriteLine(ioTCore.Identifier);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        Console.ReadLine();
    }
}