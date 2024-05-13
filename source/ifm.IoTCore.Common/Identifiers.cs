namespace ifm.IoTCore.Common;

/// <summary>
/// Defines well-known identifiers
/// </summary>
public static class Identifiers
{
    // Standard element type identifiers

    /// <summary>The device type.</summary>
    public const string Device = "device";
    /// <summary>The sub-device type.</summary>
    public const string SubDevice = "subdevice";
    /// <summary>The structure type.</summary>
    public const string Structure = "structure";
    /// <summary>The data type.</summary>
    public const string Data = "data";
    /// <summary>The service type.</summary>
    public const string Service = "service";
    /// <summary>The event type.</summary>
    public const string Event = "event";

    // Standard service identifiers

    /// <summary>The getidentity service.</summary>
    public const string GetIdentity = "getidentity";
    /// <summary>The gettree service.</summary>
    public const string GetTree = "gettree";
    /// <summary>The querytree service.</summary>
    public const string QueryTree = "querytree";
    /// <summary>The getdatamulti service.</summary>
    public const string GetDataMulti = "getdatamulti";
    /// <summary>The setdatamulti service.</summary>
    public const string SetDataMulti = "setdatamulti";
    /// <summary>The getsubscriberlist service.</summary>
    public const string GetSubscriberList = "getsubscriberlist";

    /// <summary>The getdata service.</summary>
    public const string GetData = "getdata";
    /// <summary>The setdata service.</summary>
    public const string SetData = "setdata";

    /// <summary>The subscribe service.</summary>
    public const string Subscribe = "subscribe";
    /// <summary>The unsubscribe service.</summary>
    public const string Unsubscribe = "unsubscribe";
    /// <summary>The triggerevent service.</summary>
    public const string TriggerEvent = "triggerevent";

    // Standard event identifiers
    /// <summary>The treechanged event.</summary>
    public const string TreeChanged = "treechanged";
    /// <summary>The datachanged event.</summary>
    public const string DataChanged = "datachanged";

    // Device info profile identifiers
    public const string DeviceInfo = "deviceinfo";

    // Well-known elements
    /// <summary>The device management remote folder.</summary>
    public const string Remote = "remote";
    /// <summary>The callback trigger service.</summary>
    public const string CallBackTrigger = "___callback_trigger___";
}