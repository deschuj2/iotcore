namespace ifm.IoTCore.ElementManager.Elements;

using System;
using System.Collections.Generic;
using Common;
using Common.Exceptions;
using Common.Variant;
using Contracts.Elements;
using Contracts.Elements.Formats;
using Contracts.Elements.ServiceData.Requests;
using Contracts.Elements.ServiceData.Responses;

internal abstract class DataElementBase : BaseElement, IDataElement
{
    public IEventElement DataChangedEventElement { get; set; }

    protected DataElementBase(string identifier,
        string address,
        Format format = null, 
        IEnumerable<string> profiles = null, 
        string uid = null, 
        bool isHidden = false) : base(Identifiers.Data, identifier, address, format, profiles, uid, isHidden)
    {
    }

    public bool HasDataChanged => DataChangedEventElement != null;

    public void RaiseDataChanged()
    {
        DataChangedEventElement?.Raise();
    }

    // js2023/11/17: As discussed with Marco Ertl, the OS must always provide a time to .net.
    // The time comes from a RTC or from a NTS. So this call always works and returns the time as
    // it is provided by the OS.
    public long TimeStamp => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
}

internal interface IReadDataElementBase
{
    GetDataResponseServiceData GetDataFunc(IBaseElement element, int? cid);
}

internal interface IWriteDataElementBase
{
    void SetDataFunc(IBaseElement element, SetDataRequestServiceData data, int? cid);
}

internal abstract class CachedDataElementBase<T> : DataElementBase
{
    private T _value;
    private readonly TimeSpan? _cacheTimeout;
    private DateTime? _cacheLastRefreshTime;

    protected CachedDataElementBase(string identifier,
        string address,
        T value = default,
        TimeSpan? cacheTimeout = null,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false) : base(identifier, address, format, profiles, uid, isHidden)
    {
        _value = value;
        _cacheTimeout = cacheTimeout;
        _cacheLastRefreshTime = null;

        Format ??= DataElementHelpers.CreateFormat(typeof(T));
    }

    protected virtual T InnerGetValue()
    {
        return _value;
    }

    protected virtual void InnerSetValue(T value)
    {
        _value = value;
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
        get
        {
            if (_cacheTimeout == null || _cacheLastRefreshTime == null || _cacheLastRefreshTime + _cacheTimeout < DateTime.Now)
            {
                _value = InnerGetValue();
                _cacheLastRefreshTime = DateTime.Now;
            }
            return _value;
        }
        set
        {
            if (EqualityComparer<T>.Default.Equals(_value, value)) return;
            InnerSetValue(value);
            RaiseDataChanged();
        }
    }
}

internal sealed class ReadOnlyDataElement<T> : CachedDataElementBase<T>, IReadDataElement<T>, IReadDataElementBase
{
    private readonly Func<IBaseElement, T> _getDataFunc;

    public ReadOnlyDataElement(string identifier,
        string address,
        Func<IBaseElement, T> getDataFunc,
        T value = default,
        TimeSpan? cacheTimeout = null,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false) : base(identifier, address, value, cacheTimeout, format, profiles, uid, isHidden)
    {
        _getDataFunc = getDataFunc ?? throw new ArgumentNullException(nameof(getDataFunc));
    }

    protected override T InnerGetValue()
    {
        return _getDataFunc(this);
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

internal sealed class WriteOnlyDataElement<T> : CachedDataElementBase<T>, IWriteDataElement<T>, IWriteDataElementBase
{
    private readonly Action<IBaseElement, T> _setDataFunc;

    public WriteOnlyDataElement(string identifier,
        string address,
        Action<IBaseElement, T> setDataFunc,
        T value = default,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false) : base(identifier, address, value, null, format, profiles, uid, isHidden)
    {
        _setDataFunc = setDataFunc ?? throw new ArgumentNullException(nameof(setDataFunc));
    }

    protected override void InnerSetValue(T value)
    {
        _setDataFunc.Invoke(this, value);
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

internal sealed class DataElement<T> : CachedDataElementBase<T>, IReadWriteDataElement<T>, IReadDataElementBase, IWriteDataElementBase
{
    private readonly Func<IBaseElement, T> _getDataFunc;
    private readonly Action<IBaseElement, T> _setDataFunc;

    public DataElement(string identifier,
        string address,
        Func<IBaseElement, T> getDataFunc,
        Action<IBaseElement, T> setDataFunc,
        T value = default,
        TimeSpan? cacheTimeout = null,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false) : base(identifier, address, value, cacheTimeout, format, profiles, uid, isHidden)
    {
        _getDataFunc = getDataFunc ?? throw new ArgumentNullException(nameof(getDataFunc));
        _setDataFunc = setDataFunc ?? throw new ArgumentNullException(nameof(setDataFunc));
    }

    protected override T InnerGetValue()
    {
        return _getDataFunc(this);
    }

    protected override void InnerSetValue(T value)
    {
        _setDataFunc.Invoke(this, value);
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
