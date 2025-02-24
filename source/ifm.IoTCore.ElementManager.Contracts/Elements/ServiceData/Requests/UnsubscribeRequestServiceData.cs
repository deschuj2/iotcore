﻿namespace ifm.IoTCore.ElementManager.Contracts.Elements.ServiceData.Requests;

using Common.Variant;

/// <summary>
/// Represents the incoming data for a IEventElement.Unsubscribe service call.
/// </summary>
public class UnsubscribeRequestServiceData
{
    /// <summary>
    /// The callback address of the subscription.
    /// </summary>
    [VariantProperty("callback", Required = true, AlternativeNames = new[] { "callbackurl" })]
    public string Callback { get; set; }

    /// <summary>
    /// The id which identifies the subscription.
    /// </summary>
    [VariantProperty("subscribeid", IgnoredIfNull = true)]
    public int? SubscriptionId { get; set; }

    /// <summary>
    /// The parameterless constructor for the variant converter.
    /// </summary>
    [VariantConstructor]
    public UnsubscribeRequestServiceData()
    {
    }

    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="callback">The callback address of the subscription.</param>
    /// <param name="subscriptionId">The id which identifies the subscription.</param>
    public UnsubscribeRequestServiceData(string callback, int? subscriptionId = null)
    {
        Callback = callback;
        SubscriptionId = subscriptionId;
    }
}