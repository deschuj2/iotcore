namespace Sample14;

using System;
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
            var service1 = ioTCore.ElementManager.CreateActionServiceElement(struct1,
                "service1",
                HandleService1);
            service1.AddUserData<UserData>("user_data", new UserData());
            service1.Invoke();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        Console.ReadLine();
    }

    private static void HandleService1(IBaseElement element, int? cid)
    {
        var userData = element.GetUserData<UserData>("user_data");
        Console.WriteLine($"Do something with {userData}");
    }
}

internal class UserData
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

    public override string ToString()
    {
        return $"Int1={Int1} Float1={Float1} String1={String1}";
    }
}