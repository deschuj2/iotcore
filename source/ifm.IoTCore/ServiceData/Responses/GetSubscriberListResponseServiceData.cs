namespace ifm.IoTCore.ServiceData.Responses;

using System.Collections;
using System.Collections.Generic;
using Common.Variant;

/// <summary>
/// Represents a subscription on an event element.
/// </summary>
public class GetSubscriberListItem
{
    /// <summary>
    /// The address of the event element.
    /// </summary>
    [VariantProperty("adr", IgnoredIfNull = true)]
    public string Address { get; set; }

    /// <summary>
    /// The url to which the IoTCore is sending events.
    /// </summary>
    [VariantProperty("callbackurl", Required = true)]
    public string Callback { get; set; }

    /// <summary>
    /// List of data element addresses, whose values are sent with the event.
    /// </summary>
    [VariantProperty("datatosend", IgnoredIfNull = true)]
    public List<string> DataToSend { get; set; }

    /// <summary>
    /// Specifies the persistence duration type.
    /// </summary>
    [VariantProperty("persist", IgnoredIfNull = true)]
    public bool Persist { get; set; }

    /// <summary>
    /// The id which identifies the subscription.
    /// </summary>
    [VariantProperty("subscribeid", IgnoredIfNull = true)]
    public int SubscriptionId { get; set; }

    /// <summary>
    /// The old id which identifies the subscription.
    /// </summary>
    [VariantProperty("cid", IgnoredIfNull = true)]
    public int Cid { get => SubscriptionId; set => SubscriptionId = value; }

    /// <summary>
    /// The parameterless constructor for the variant converter.
    /// </summary>
    [VariantConstructor]
    public GetSubscriberListItem()
    {
    }

    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="address">The address of the event element.</param>
    /// <param name="callback">The url to which the IoTCore is sending events.</param>
    /// <param name="dataToSend">List of data element addresses, whose values are sent with the event.</param>
    /// <param name="persist">Specifies the persistence duration type.</param>
    /// <param name="subscriptionId">The uid which identifies the subscription.</param>
    public GetSubscriberListItem(string address, string callback, List<string> dataToSend, bool persist, int subscriptionId)
    {
        Address = address;
        Callback = callback;
        DataToSend = dataToSend;
        Persist = persist;
        SubscriptionId = subscriptionId;
    }
}

/// <summary>
/// Represents the outgoing data for a IDeviceElement.GetSubscriberList service call.
/// </summary>
public class GetSubscriberListResponseServiceData : IEnumerable<GetSubscriberListItem>
{
    private readonly List<GetSubscriberListItem> _subscriptions = new();

    /// <summary>
    /// Adds a new subscription info item.
    /// </summary>
    /// <param name="item">The item to add.</param>
    public void Add(GetSubscriberListItem item)
    {
        _subscriptions.Add(item);
    }

    /// <summary>
    /// Add a new subscription info item.
    /// </summary>
    /// <param name="address">The address of the event element.</param>
    /// <param name="callback">The url to which the IoTCore is sending events.</param>
    /// <param name="dataToSend">List of data element addresses, whose values are sent with the event.</param>
    /// <param name="persist">Specifies the persistence duration type.</param>
    /// <param name="sid">The id which identifies the subscription.</param>
    public void Add(string address, string callback, List<string> dataToSend, bool persist, int sid)
    {
        _subscriptions.Add(new GetSubscriberListItem(address,
            callback,
            dataToSend,
            persist,
            sid));
    }

    /// <summary>
    /// Gets the enumerator.
    /// </summary>
    /// <returns>The enumerator.</returns>
    public IEnumerator<GetSubscriberListItem> GetEnumerator()
    {
        return _subscriptions.GetEnumerator();
    }

    /// <summary>
    /// Gets the enumerator.
    /// </summary>
    /// <returns>The enumerator.</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_subscriptions).GetEnumerator();
    }

    /// <summary>
    /// Gets or sets the element at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the element to get or set.</param>
    /// <returns>The element at the specified index.</returns>
    public GetSubscriberListItem this[int index]
    {
        get => _subscriptions[index];
        set => _subscriptions[index] = value;
    }
}