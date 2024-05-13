namespace ifm.IoTCore.ElementManager.Contracts.Elements;

using Common.Variant;
using ServiceData.Requests;
using ServiceData.Responses;

/// <summary>
/// Provides functionality to interact with a data element.
/// </summary>
public interface IDataElement : IBaseElement
{
    /// <summary>
    /// Gets the datachanged event element.
    /// </summary>
    IEventElement DataChangedEventElement { get; }

    // Returns true if data changed event exists; otherwise false.
    bool HasDataChanged { get; }

    // Raises a data changed event.
    void RaiseDataChanged();
}

/// <summary>
/// Provides functionality to interact with a readable data element.
/// </summary>
public interface IReadDataElement : IDataElement
{
    /// <summary>
    /// Gets the value.
    /// </summary>
    Variant GetValue();

    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <returns>The getdata response.</returns>
    GetDataResponseServiceData GetData();
}

public interface IReadDataElement<out T> : IReadDataElement
{
    /// <summary>
    /// Gets the value.
    /// </summary>
    T Value { get; }
}

/// <summary>
/// Provides functionality to interact with a writable data element.
/// </summary>
public interface IWriteDataElement : IDataElement
{
    /// <summary>
    /// Sets the value.
    /// </summary>
    void SetValue(Variant value);

    /// <summary>
    /// Sets the data.
    /// </summary>
    /// <param name="data">The setdata request.</param>
    void SetData(SetDataRequestServiceData data);

}

public interface IWriteDataElement<in T> : IWriteDataElement
{
    /// <summary>
    /// Sets the value.
    /// </summary>
    T Value { set; }
}

/// <summary>
/// Provides functionality to interact with a readable and writable data element.
/// </summary>
public interface IReadWriteDataElement : IReadDataElement, IWriteDataElement
{
}

/// <summary>
/// Provides functionality to interact with a readable and writable data element.
/// </summary>
public interface IReadWriteDataElement<T> : IReadWriteDataElement, IReadDataElement<T>, IWriteDataElement<T>
{
    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    new T Value { get; set; }
}
