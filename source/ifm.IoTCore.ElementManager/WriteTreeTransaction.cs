namespace ifm.IoTCore.ElementManager;

using System;
using System.Collections.Generic;
using Contracts;
using Contracts.Elements;
using Contracts.Elements.Formats;
using Contracts.Elements.Tree;

internal class WriteTreeTransaction : IWriteTreeTransaction
{
    private readonly IElementManager _elementManager;

    public WriteTreeTransaction(IElementManager elementManager)
    {
        _elementManager = elementManager ?? throw new ArgumentNullException(nameof(elementManager));
    }

    public void Begin()
    {
        _elementManager.EnterWriteLock();
    }

    public void End()
    {
        if (_elementManager.IsWriteLockHeld)
        {
            _elementManager.ExitWriteLock();
        }

        _elementManager.RaiseTreeChanged(TreeChangedActions.TreeChanged, _elementManager.Root, null);
    }

    public IBaseElement CreateElement(IBaseElement parentElement, string type, string identifier, Format format = null,
        IEnumerable<string> profiles = null, string uid = null, bool isHidden = false)
    {
        return _elementManager.CreateElement(parentElement, type, identifier, format, profiles, uid, isHidden, false, false);
    }

    public IStructureElement CreateStructureElement(IBaseElement parentElement, string identifier, Format format = null,
        IEnumerable<string> profiles = null, string uid = null, bool isHidden = false)
    {
        return _elementManager.CreateStructureElement(parentElement, identifier, format, profiles, uid, isHidden, false, false);
    }

    public IActionServiceElement CreateActionServiceElement(IBaseElement parentElement, string identifier, Action<IBaseElement, int?> func,
        Format format = null, IEnumerable<string> profiles = null, string uid = null, bool isHidden = false)
    {
        return _elementManager.CreateActionServiceElement(parentElement, identifier, func, format, profiles, uid, isHidden, false, false);
    }

    public IGetterServiceElement<TOut> CreateGetterServiceElement<TOut>(IBaseElement parentElement, string identifier, Func<IBaseElement, int?, TOut> func,
        Format format = null, IEnumerable<string> profiles = null, string uid = null, bool isHidden = false)
    {
        return _elementManager.CreateGetterServiceElement(parentElement, identifier, func, format, profiles, uid, isHidden, false, false);
    }

    public ISetterServiceElement<TIn> CreateSetterServiceElement<TIn>(IBaseElement parentElement, string identifier, Action<IBaseElement, TIn, int?> func,
        Format format = null, IEnumerable<string> profiles = null, string uid = null, bool isHidden = false)
    {
        return _elementManager.CreateSetterServiceElement(parentElement, identifier, func, format, profiles, uid, isHidden, false, false);
    }

    public IServiceElement<TIn, TOut> CreateServiceElement<TIn, TOut>(IBaseElement parentElement, string identifier, Func<IBaseElement, TIn, int?, TOut> func,
        Format format = null, IEnumerable<string> profiles = null, string uid = null, bool isHidden = false)
    {
        return _elementManager.CreateServiceElement(parentElement, identifier, func, format, profiles, uid, isHidden, false, false);
    }

    public IEventElement CreateEventElement(IBaseElement parentElement, string identifier, Format format = null,
        IEnumerable<string> profiles = null, string uid = null, bool isHidden = false)
    {
        return _elementManager.CreateEventElement(parentElement, identifier, format, profiles, uid, isHidden, false, false);
    }

    public IReadWriteDataElement<T> CreateSimpleDataElement<T>(IBaseElement parentElement, string identifier, T value = default,
        Format format = null, IEnumerable<string> profiles = null, string uid = null, bool isHidden = false)
    {
        return _elementManager.CreateSimpleDataElement(parentElement, identifier, value, format, profiles, uid, isHidden, false, false);
    }

    public IReadWriteDataElement<T> CreateSimpleDataElement<T>(IBaseElement parentElement, string identifier,
        bool createDataChangedEventElement, T value = default, Format format = null, IEnumerable<string> profiles = null,
        string uid = null, bool isHidden = false)
    {
        return _elementManager.CreateSimpleDataElement(parentElement, identifier, createDataChangedEventElement, value, format, profiles, uid, isHidden, false, false);
    }

    public IReadWriteDataElement<T> CreateSimpleDataElement<T>(IBaseElement parentElement, string identifier,
        bool createGetDataServiceElement, bool createSetDataServiceElement, T value = default, Format format = null,
        IEnumerable<string> profiles = null, string uid = null, bool isHidden = false)
    {
        return _elementManager.CreateSimpleDataElement(parentElement, identifier, createGetDataServiceElement, createSetDataServiceElement, value, format, profiles, uid, isHidden, false, false);
    }

    public IReadWriteDataElement<T> CreateSimpleDataElement<T>(IBaseElement parentElement, string identifier,
        bool createGetDataServiceElement, bool createSetDataServiceElement, bool createDataChangedEventElement,
        T value = default, Format format = null, IEnumerable<string> profiles = null, string uid = null, bool isHidden = false)
    {
        return _elementManager.CreateSimpleDataElement(parentElement, identifier, createGetDataServiceElement, createSetDataServiceElement, createDataChangedEventElement, value, format, profiles, uid, isHidden, false, false);
    }

    public IReadDataElement<T> CreateReadOnlyDataElement<T>(IBaseElement parentElement, string identifier, Func<IBaseElement, T> getDataFunc,
        T value = default, TimeSpan? cacheTimeout = null, Format format = null, IEnumerable<string> profiles = null,
        string uid = null, bool isHidden = false)
    {
        return _elementManager.CreateReadOnlyDataElement(parentElement, identifier, getDataFunc, value, cacheTimeout, format, profiles, uid, isHidden, false, false);
    }

    public IReadDataElement<T> CreateReadOnlyDataElement<T>(IBaseElement parentElement, string identifier, Func<IBaseElement, T> getDataFunc,
        bool createDataChangedEventElement, T value = default, TimeSpan? cacheTimeout = null, Format format = null,
        IEnumerable<string> profiles = null, string uid = null, bool isHidden = false)
    {
        return _elementManager.CreateReadOnlyDataElement(parentElement, identifier, getDataFunc, createDataChangedEventElement, value, cacheTimeout, format, profiles, uid, isHidden, false, false);
    }

    public IWriteDataElement<T> CreateWriteOnlyDataElement<T>(IBaseElement parentElement, string identifier, Action<IBaseElement, T> setDataFunc,
        Format format = null, IEnumerable<string> profiles = null, string uid = null, bool isHidden = false)
    {
        return _elementManager.CreateWriteOnlyDataElement(parentElement, identifier, setDataFunc, format, profiles, uid, isHidden, false, false);
    }

    public IWriteDataElement<T> CreateWriteOnlyDataElement<T>(IBaseElement parentElement, string identifier, Action<IBaseElement, T> setDataFunc,
        bool createDataChangedEventElement, Format format = null, IEnumerable<string> profiles = null, string uid = null,
        bool isHidden = false)
    {
        return _elementManager.CreateWriteOnlyDataElement(parentElement, identifier, setDataFunc, createDataChangedEventElement, format, profiles, uid, isHidden, false, false);
    }

    public IReadWriteDataElement<T> CreateDataElement<T>(IBaseElement parentElement, string identifier, Func<IBaseElement, T> getDataFunc,
        Action<IBaseElement, T> setDataFunc, T value = default, TimeSpan? cacheTimeout = null, Format format = null,
        IEnumerable<string> profiles = null, string uid = null, bool isHidden = false)
    {
        return _elementManager.CreateDataElement(parentElement, identifier, getDataFunc, setDataFunc, value, cacheTimeout, format, profiles, uid, isHidden, false, false);
    }

    public IReadWriteDataElement<T> CreateDataElement<T>(IBaseElement parentElement, string identifier, Func<IBaseElement, T> getDataFunc,
        Action<IBaseElement, T> setDataFunc, bool createDataChangedEventElement, T value = default, TimeSpan? cacheTimeout = null,
        Format format = null, IEnumerable<string> profiles = null, string uid = null, bool isHidden = false)
    {
        return _elementManager.CreateDataElement(parentElement, identifier, getDataFunc, setDataFunc, createDataChangedEventElement, value, cacheTimeout, format, profiles, uid, isHidden, false, false);
    }

    public void AddElement(IBaseElement parentElement, IBaseElement element)
    {
        _elementManager.AddElement(parentElement, element, false, false);
    }

    public void RemoveElement(IBaseElement parentElement, IBaseElement element)
    {
        _elementManager.RemoveElement(parentElement, element, false, false);
    }

    public void AddLink(IBaseElement sourceElement, IBaseElement targetElement, string identifier = null)
    {
        _elementManager.AddLink(sourceElement, targetElement, identifier, false, false);
    }

    public void RemoveLink(IBaseElement sourceElement, IBaseElement targetElement)
    {
        _elementManager.RemoveLink(sourceElement, targetElement, false, false);
    }

    public void RemoveLink(IBaseElement sourceElement, string identifier)
    {
        _elementManager.RemoveLink(sourceElement, identifier, false, false);
    }
}