namespace Sample07;

using System;
using ifm.IoTCore.Common;
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
            ioTCore.ElementManager.CreateDataElement<float>(struct1, 
                "float1",
                GetFloat1, 
                SetFloat1,
                format: new FloatFormat(new FloatValuation(-9.9f, 9.9f, 3)));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        Console.ReadLine();
    }

    private static float GetFloat1(IBaseElement element)
    {
        return _float1;
    }

    private static void SetFloat1(IBaseElement element, float value)
    {
        if (_float1.EqualsWithPrecision(value)) return;
        _float1 = value;
    }
    private static float _float1 = 1.2345f;
}