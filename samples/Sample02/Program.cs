namespace Sample02;

using System;
using System.Collections.Generic;
using ifm.IoTCore.Factory;
    
internal class Program
{
    static void Main()
    {
        try
        {
            var ioTCore = IoTCoreFactory.Create("MyIoTCore");
                
            ioTCore.ElementManager.CreateStructureElement(ioTCore.Root, 
                "struct1", 
                null, 
                new List<string> { "profile1" });
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        Console.ReadLine();
    }
}