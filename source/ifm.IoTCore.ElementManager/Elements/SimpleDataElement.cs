namespace ifm.IoTCore.ElementManager.Elements;

using System.Collections.Generic;
using Common.Exceptions;
using Common.Variant;
using Contracts.Elements;
using Contracts.Elements.Formats;
using Contracts.Elements.ServiceData.Requests;
using Contracts.Elements.ServiceData.Responses;

internal abstract class SimpleDataElementBase<T> : DataElementBase
{
    private T _value;

    protected SimpleDataElementBase(string identifier,
        string address,
        T value = default,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false) : base(identifier, address, format, profiles, uid, isHidden)
    {
        _value = value;

        Format ??= DataElementHelpers.CreateFormat(typeof(T));
    }

    public Variant GetValue()
    {
        return Variant.FromObject(Value);
    }


    public void SetValue(Variant value)
    {
        Value = Variant.ToObject<T>(value);
    }

    public T Value
    {
        get => _value;
        set
        {
            if (EqualityComparer<T>.Default.Equals(_value, value)) return;
            _value = value;
            RaiseDataChanged();
        }
    }
}

internal sealed class SimpleReadOnlyDataElement<T> : SimpleDataElementBase<T>, IReadDataElement<T>, IReadDataElementBase
{
    public SimpleReadOnlyDataElement(string identifier,
        string address,
        T value = default,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false) : base(identifier, address, value, format, profiles, uid, isHidden)
    {
    }

    public GetDataResponseServiceData GetDataFunc(IBaseElement element, int? cid)
    {
        return new GetDataResponseServiceData(GetValue(), TimeStamp);
    }
    public GetDataResponseServiceData GetData()
    {
        return new GetDataResponseServiceData(GetValue(), TimeStamp);
    }

}

internal sealed class SimpleWriteOnlyDataElement<T> : SimpleDataElementBase<T>, IWriteDataElement<T>, IWriteDataElementBase
{
    public SimpleWriteOnlyDataElement(string identifier,
        string address,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false) : base(identifier, address, default, format, profiles, uid, isHidden)
    {
    }

    public void SetDataFunc(IBaseElement element, SetDataRequestServiceData data, int? cid)
    {
        if (data == null) throw new DataInvalidException("Service data empty");
        SetValue(data.Value);
    }

    public void SetData(SetDataRequestServiceData data)
    {
        if (data == null) throw new DataInvalidException("Service data empty");
        SetValue(data.Value);
    }
}

internal sealed class SimpleDataElement<T> : SimpleDataElementBase<T>, IReadWriteDataElement<T>, IReadDataElementBase, IWriteDataElementBase
{
    public SimpleDataElement(string identifier,
        string address,
        T value = default,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false) : base(identifier, address, value, format, profiles, uid, isHidden)
    {
    }

    public GetDataResponseServiceData GetDataFunc(IBaseElement element, int? cid)
    {
        return new GetDataResponseServiceData(GetValue(), TimeStamp);
    }

    public void SetDataFunc(IBaseElement element, SetDataRequestServiceData data, int? cid)
    {
        if (data == null) throw new DataInvalidException("Service data empty");
        SetValue(data.Value);
    }

    public GetDataResponseServiceData GetData()
    {
        return new GetDataResponseServiceData(GetValue(), TimeStamp);
    }

    public void SetData(SetDataRequestServiceData data)
    {
        if (data == null) throw new DataInvalidException("Service data empty");
        SetValue(data.Value);
    }
}
