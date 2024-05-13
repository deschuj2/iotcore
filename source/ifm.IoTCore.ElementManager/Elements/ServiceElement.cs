namespace ifm.IoTCore.ElementManager.Elements;

using System;
using System.Collections.Generic;
using Common;
using Common.Exceptions;
using Common.Variant;
using Contracts.Elements;
using Contracts.Elements.Formats;

internal abstract class ServiceElementBase : BaseElement, IServiceElement
{
    protected ServiceElementBase(string identifier,
        string address,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false) : base(Identifiers.Service, identifier, address, format, profiles, uid, isHidden)
    {
    }

    public abstract Variant Invoke(Variant data, int? cid);

    protected static T VariantToObject<T>(Variant data)
    {
        try
        {
            return data != null ? Variant.ToObject<T>(data) : default;
        }
        catch (Exception e)
        {
            throw new DataInvalidException(e.Message);
        }
    }

    protected static Variant VariantFromObject<T>(T data)
    {
        try
        {
            return data != null ? Variant.FromObject(data) : null;
        }
        catch (Exception e)
        {
            throw new DataInvalidException(e.Message);
        }
    }
}

internal sealed class ActionServiceElement : ServiceElementBase, IActionServiceElement
{
    private readonly Action<IServiceElement, int?> _func;

    public ActionServiceElement(string identifier,
        string address,
        Action<IBaseElement, int?> func,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false) :
        base(identifier, address, format, profiles, uid, isHidden)
    {
        _func = func ?? throw new ArgumentNullException(nameof(func));
    }

    public void Invoke(int? cid = null)
    {
        _func(this, cid);
    }

    public override Variant Invoke(Variant data, int? cid)
    {
        _func(this, cid);
        return null;
    }
}

internal sealed class GetterServiceElement<TOut> : ServiceElementBase, IGetterServiceElement<TOut>
{
    private readonly Func<IServiceElement, int?, TOut> _func;

    public GetterServiceElement(string identifier,
        string address,
        Func<IBaseElement, int?, TOut> func,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false) :
        base(identifier, address, format, profiles, uid, isHidden)
    {
        _func = func ?? throw new ArgumentNullException(nameof(func));
    }

    public TOut Invoke(int? cid = null)
    {
        return _func(this, cid);
    }

    public override Variant Invoke(Variant data, int? cid)
    {
        return VariantFromObject(_func(this, cid));
    }
}

internal sealed class SetterServiceElement<TIn> : ServiceElementBase, ISetterServiceElement<TIn>
{
    private readonly Action<IServiceElement, TIn, int?> _func;

    public SetterServiceElement(string identifier,
        string address,
        Action<IBaseElement, TIn, int?> func,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false) :
        base(identifier, address, format, profiles, uid, isHidden)
    {
        _func = func ?? throw new ArgumentNullException(nameof(func));
    }

    public void Invoke(TIn data, int? cid = null)
    {
        _func(this, data, cid);
    }

    public override Variant Invoke(Variant data, int? cid)
    {
        _func(this, VariantToObject<TIn>(data), cid);
        return null;
    }
}

internal sealed class ServiceElement<TIn, TOut> : ServiceElementBase, IServiceElement<TIn, TOut>
{
    private readonly Func<IServiceElement, TIn, int?, TOut> _func;

    public ServiceElement(string identifier,
        string address,
        Func<IBaseElement, TIn, int?, TOut> func,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false) :
        base(identifier, address, format, profiles, uid, isHidden)
    {
        _func = func ?? throw new ArgumentNullException(nameof(func));
    }

    public TOut Invoke(TIn data, int? cid = null)
    {
        return _func(this, data, cid);
    }

    public override Variant Invoke(Variant data, int? cid)
    {
        return VariantFromObject(_func(this, VariantToObject<TIn>(data), cid));
    }
}
