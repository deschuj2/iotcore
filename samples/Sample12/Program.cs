namespace Sample12;

using System;
using System.Collections.Generic;
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
            var intField = new ObjectValuation.Field("intField1",
                new Int32Format(new Int32Valuation(-100, 100)));
            var floatField = new ObjectValuation.Field("floatField1",
                new FloatFormat(new FloatValuation(-100.0f, 100.0f, 3)));
            var stringField = new ObjectValuation.Field("stringField1",
                new StringFormat(new StringValuation(10, 10, "dd-mm-yyyy")));
            ioTCore.ElementManager.CreateDataElement<UserData>(struct1, 
                "object1",
                GetObject1, 
                SetObject1,
                format: new ObjectFormat(new ObjectValuation(new List<ObjectValuation.Field>
                {
                    intField, 
                    floatField, 
                    stringField
                })));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        Console.ReadLine();
    }

    private static UserData GetObject1(IBaseElement element)
    {
        return _object1;
    }

    private static void SetObject1(IBaseElement element, UserData value)
    {
        if (_object1.Equals(value)) return;
        _object1 = value;
    }
    private static UserData _object1 = new UserData();
}

internal class UserData : IEquatable<UserData>
{
    public string String1;
    public int Int1;
    public float Float1;

    public UserData()
    {
        String1 = "Hallo";
        Int1 = 10;
        Float1 = 1.2345f;
    }

    public bool Equals(UserData other)
    {
        if (this == other) return true;
        if (other == null) return false;
        return Int1 == other.Int1 &&
               Float1.Equals(other.Float1) &&
               String1 == other.String1;
    }
}