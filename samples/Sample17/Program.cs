namespace Sample17;

using System;
using ifm.IoTCore.Common.Variant;
using System.Collections.Generic;
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

            var struct1 = ioTCore.ElementManager.CreateStructureElement(ioTCore.Root, "struct1");

            ioTCore.ElementManager.CreateSetterServiceElement<EventServiceData>(struct1, 
                "service1", 
                HandleService1);

            ioTCore.ElementManager.CreateReadOnlyDataElement<string>(struct1, 
                "string1", 
                GetString1, 
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
    private static string _string1 = "Hallo";

    private static void HandleService1(IBaseElement element,
        EventServiceData data, 
        int? cid = null)
    {
        Console.WriteLine("HandleService1 called");

        Console.WriteLine($"Event source={data.EventAddress}");
        Console.WriteLine($"Event number={data.EventNumber}");
        foreach (var (key, value) in data.Payload)
        {
            Console.WriteLine($"{key}={value.Code},{value.Data}");
        }
    }
}

internal class EventServiceData
{
    [VariantProperty("eventno", IgnoredIfNull = true)]
    public int EventNumber { get; set; }

    [VariantProperty("srcurl", IgnoredIfNull = true)]
    public string EventAddress { get; set; }

    [VariantProperty("payload", IgnoredIfNull = true)]
    public Dictionary<string, CodeDataPair> Payload { get; set; }

    [VariantProperty("subscribeid", IgnoredIfNull = true)]
    public int? SubscriptionId { get; set; }

    [VariantConstructor]
    public EventServiceData()
    {
    }

    public EventServiceData(int eventNumber, string eventAddress, Dictionary<string, CodeDataPair> payload, int? subscriptionId = null)
    {
        EventNumber = eventNumber;
        EventAddress = eventAddress;
        Payload = payload;
        SubscriptionId = subscriptionId;
    }
}