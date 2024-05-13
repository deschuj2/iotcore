namespace Sample11;

using System;
using System.Linq;
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
            var array1 = ioTCore.ElementManager.CreateDataElement<int[]>(struct1, 
                "array1",
                GetArray1, 
                SetArray1,
                format: 
                new ArrayFormat(
                    new ArrayValuation(
                        new Int32Format(new Int32Valuation(0, 100)))));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        Console.ReadLine();
    }

    private static int[] GetArray1(IBaseElement element)
    {
        return _array1;
    }

    private static void SetArray1(IBaseElement element, int[] value)
    {
        if (_array1.SequenceEqual(value)) return;
        _array1 = value;
    }
    private static int[] _array1 = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
}