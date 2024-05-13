namespace Sample08;

using System;
using ifm.IoTCore.ElementManager.Contracts.Elements;
using ifm.IoTCore.ElementManager.Contracts.Elements.Formats;
using ifm.IoTCore.ElementManager.Contracts.Elements.Valuations;
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
            ioTCore.ElementManager.CreateDataElement<string>(struct1, "string1",
                GetString1, 
                SetString1,
                format: new StringFormat(new StringValuation(0, 100)));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        Console.ReadLine();
    }

    private static string GetString1(IBaseElement element)
    {
        return _string1;
    }

    private static void SetString1(IBaseElement element, string value)
    {
        _string1 = value;
    }
    private static string _string1 = "Hallo";
}