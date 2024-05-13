namespace Sample10;

using System;
using System.Diagnostics;
using ifm.IoTCore.ElementManager.Contracts.Elements;
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
            var struct2 = ioTCore.ElementManager.CreateStructureElement(struct1, 
                "struct1");
            var data1 = ioTCore.ElementManager.CreateSimpleDataElement<int>(struct2, 
                "data1", value:123);

            // Create a link from root element to data1 element
            ioTCore.ElementManager.AddLink(ioTCore.Root, 
                data1, 
                "link_data1");

            // Get the element via link
            var element = (IReadDataElement)ioTCore.ElementManager.GetElementByAddress("/link_data1");
            Console.WriteLine(element.GetValue());

            // Remove the link from root element to data1 element
            ioTCore.ElementManager.RemoveLink(ioTCore.Root, data1);
            element = (IReadDataElement)ioTCore.ElementManager.GetElementByAddress("/link_data1");
            Debug.Assert(element == null);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        Console.ReadLine();
    }
}