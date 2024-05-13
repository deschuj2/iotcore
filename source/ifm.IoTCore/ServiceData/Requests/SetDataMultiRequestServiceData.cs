namespace ifm.IoTCore.ServiceData.Requests;

using System.Collections.Generic;
using Common.Variant;

/// <summary>
/// Represents the incoming data for a IDeviceElement.SetDataMulti service call.
/// </summary>
public class SetDataMultiRequestServiceData
{
    /// <summary>
    /// List of addresses of data elements and the values to set.
    /// </summary>
    [VariantProperty("datatosend", Required = true)]
    public Dictionary<string, Variant> DataToSend { get; set; }

    /// <summary>
    /// The parameterless constructor for the variant converter.
    /// </summary>
    [VariantConstructor]
    public SetDataMultiRequestServiceData()
    {
    }

    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="dataToSend">List of addresses of data elements and the values to set.</param>
    public SetDataMultiRequestServiceData(Dictionary<string, Variant> dataToSend)
    {
        DataToSend = dataToSend;
    }
}