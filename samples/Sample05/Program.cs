namespace Sample05;

using System;
using ifm.IoTCore.ElementManager.Contracts.Elements;
using ifm.IoTCore.ElementManager.Contracts.Elements.Formats;
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
            ioTCore.ElementManager.CreateDataElement<bool>(struct1, 
                "bool1",
                GetBool1, 
                SetBool1,
                format: new BooleanFormat());
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        Console.ReadLine();
    }

    private static bool GetBool1(IBaseElement element)
    {
        return _bool1;
    }

    private static void SetBool1(IBaseElement element, bool value)
    {
        _bool1 = value;
    }
    private static bool _bool1 = true;
}