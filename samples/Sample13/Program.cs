namespace Sample13;

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

            ioTCore.ElementManager.CreateSimpleDataElement<string>(struct1, 
                "string1");

            var string1 = ioTCore.ElementManager.GetElementByAddress("/struct1/string1");
            Console.WriteLine(string1.Address);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        Console.ReadLine();
    }
}