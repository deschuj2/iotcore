namespace Sample06;

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
            ioTCore.ElementManager.CreateDataElement<int>(struct1, 
                "int1",
                GetInt1, 
                SetInt1,
                format: new Int32Format(new Int32Valuation(0, 100)));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        Console.ReadLine();
    }

    private static int GetInt1(IBaseElement element)
    {
        return _int1;
    }

    private static void SetInt1(IBaseElement element, int value)
    {
        _int1 = value;
    }
    private static int _int1 = 10;
}