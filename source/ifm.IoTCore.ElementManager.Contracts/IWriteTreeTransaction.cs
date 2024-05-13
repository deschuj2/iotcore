namespace ifm.IoTCore.ElementManager.Contracts;

using System;
using System.Collections.Generic;
using Elements;
using Elements.Formats;

/// <summary>
/// Provides functionality to interact with an element write tree transaction.
/// </summary>
public interface IWriteTreeTransaction : ITreeTransaction
{
    IBaseElement CreateElement(IBaseElement parentElement,
        string type,
        string identifier,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false);

    IStructureElement CreateStructureElement(IBaseElement parentElement,
        string identifier,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false);

    IActionServiceElement CreateActionServiceElement(IBaseElement parentElement,
        string identifier,
        Action<IBaseElement, int?> func,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false);

    IGetterServiceElement<TOut> CreateGetterServiceElement<TOut>(IBaseElement parentElement,
        string identifier,
        Func<IBaseElement, int?, TOut> func,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false);

    ISetterServiceElement<TIn> CreateSetterServiceElement<TIn>(IBaseElement parentElement,
        string identifier,
        Action<IBaseElement, TIn, int?> func,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false);

    IServiceElement<TIn, TOut> CreateServiceElement<TIn, TOut>(IBaseElement parentElement,
        string identifier,
        Func<IBaseElement, TIn, int?, TOut> func,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false);

    IEventElement CreateEventElement(IBaseElement parentElement,
        string identifier,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false);

    IReadWriteDataElement<T> CreateSimpleDataElement<T>(IBaseElement parentElement,
        string identifier,
        T value = default,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false);

    IReadWriteDataElement<T> CreateSimpleDataElement<T>(IBaseElement parentElement,
        string identifier,
        bool createDataChangedEventElement,
        T value = default,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false);

    IReadWriteDataElement<T> CreateSimpleDataElement<T>(IBaseElement parentElement,
        string identifier,
        bool createGetDataServiceElement,
        bool createSetDataServiceElement,
        T value = default,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false);

    IReadWriteDataElement<T> CreateSimpleDataElement<T>(IBaseElement parentElement,
        string identifier,
        bool createGetDataServiceElement,
        bool createSetDataServiceElement,
        bool createDataChangedEventElement,
        T value = default,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false);

    IReadDataElement<T> CreateReadOnlyDataElement<T>(IBaseElement parentElement,
        string identifier,
        Func<IBaseElement, T> getDataFunc,
        T value = default,
        TimeSpan? cacheTimeout = null,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false);

    IReadDataElement<T> CreateReadOnlyDataElement<T>(IBaseElement parentElement,
        string identifier,
        Func<IBaseElement, T> getDataFunc,
        bool createDataChangedEventElement,
        T value = default,
        TimeSpan? cacheTimeout = null,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false);

    IWriteDataElement<T> CreateWriteOnlyDataElement<T>(IBaseElement parentElement,
        string identifier,
        Action<IBaseElement, T> setDataFunc,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false);

    IWriteDataElement<T> CreateWriteOnlyDataElement<T>(IBaseElement parentElement,
        string identifier,
        Action<IBaseElement, T> setDataFunc,
        bool createDataChangedEventElement,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false);

    IReadWriteDataElement<T> CreateDataElement<T>(IBaseElement parentElement,
        string identifier,
        Func<IBaseElement, T> getDataFunc,
        Action<IBaseElement, T> setDataFunc,
        T value = default,
        TimeSpan? cacheTimeout = null,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false);

    IReadWriteDataElement<T> CreateDataElement<T>(IBaseElement parentElement,
        string identifier,
        Func<IBaseElement, T> getDataFunc,
        Action<IBaseElement, T> setDataFunc,
        bool createDataChangedEventElement,
        T value = default,
        TimeSpan? cacheTimeout = null,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false);

    void AddElement(IBaseElement parentElement, IBaseElement element);

    void RemoveElement(IBaseElement parentElement, IBaseElement element);

    void AddLink(IBaseElement sourceElement, IBaseElement targetElement, string identifier = null);

    void RemoveLink(IBaseElement sourceElement, IBaseElement targetElement);
}