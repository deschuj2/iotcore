﻿namespace ifm.IoTCore.ElementManager.Contracts;

using System;
using System.Collections.Generic;
using Elements;
using Elements.Formats;
using Elements.Tree;

/// <summary>
/// Provides functionality to interact with an element manager.
/// </summary>
public interface IElementManager : IElementCache
{
    /// <summary>
    /// Enters the read lock.
    /// </summary>
    void EnterReadLock();

    /// <summary>
    /// Exits the read lock.
    /// </summary>
    void ExitReadLock();

    /// <summary>
    ///  Enters the write lock.
    /// </summary>
    void EnterWriteLock();

    /// <summary>
    /// Exists the write lock.
    /// </summary>
    void ExitWriteLock();

    /// <summary>
    /// Returns true if a read lock is held; otherwise false.
    /// </summary>
    bool IsReadLockHeld { get; }

    /// <summary>
    /// Returns true if a write lock is held; otherwise false.
    /// </summary>
    bool IsWriteLockHeld { get; }

    /// <summary>
    /// Raises a treechanged event.
    /// </summary>
    /// <param name="action">The action that caused the event.</param>
    /// <param name="sourceElement">The element that was affected by the change.</param>
    /// <param name="targetElement">The element involved in the change.</param>
    /// <param name="identifier">The identifier for a link, if it is a link event.</param>
    void RaiseTreeChanged(TreeChangedActions action,
        IBaseElement sourceElement,
        IBaseElement targetElement,
        string identifier = null);

    /// <summary>
    /// Creates a IWriteTreeTransaction object.
    /// </summary>
    /// <returns>The created object.</returns>
    IWriteTreeTransaction CreateWriteTreeTransaction();

    /// <summary>
    /// Creates a IReadTreeTransaction object.
    /// </summary>
    /// <returns>The created object.</returns>
    IReadTreeTransaction CreateReadTreeTransaction();

    /// <summary>
    /// Gets or sets the root device element.
    /// </summary>
    IBaseElement Root { get; set; }

    /// <summary>
    /// Creates an element.
    /// </summary>
    /// <param name="parentElement">The parent element. Use null if the element has not yet a parent.</param>
    /// <param name="type">The type of the element.</param>
    /// <param name="identifier">The identifier of the element.</param>
    /// <param name="format">The format of the element.</param>
    /// <param name="profiles">The profiles of the element.</param>
    /// <param name="uid">The unique identifier of the element.</param>
    /// <param name="isHidden">The hidden status of the element.</param>
    /// <param name="raiseTreeChanged">If true a treechanged event is raised; otherwise not</param>
    /// <param name="acquireLock">If true a write lock is acquired; otherwise not</param>
    /// <returns>The created element.</returns>
    IBaseElement CreateElement(IBaseElement parentElement,
        string type,
        string identifier,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false,
        bool raiseTreeChanged = false,
        bool acquireLock = true);

    /// <summary>
    /// Creates a structure element.
    /// </summary>
    /// <param name="parentElement">The parent element. Use null if the element has not yet a parent.</param>
    /// <param name="identifier">The identifier of the element.</param>
    /// <param name="format">The format of the element.</param>
    /// <param name="profiles">The profiles of the element.</param>
    /// <param name="uid">The unique identifier of the element.</param>
    /// <param name="isHidden">The hidden status of the element.</param>
    /// <param name="raiseTreeChanged">If true a treechanged event is raised; otherwise not</param>
    /// <param name="acquireLock">If true a write lock is acquired; otherwise not</param>
    /// <returns>The created element.</returns>
    IStructureElement CreateStructureElement(IBaseElement parentElement,
        string identifier,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false,
        bool raiseTreeChanged = false,
        bool acquireLock = true);

    /// <summary>
    /// Creates an action service element.
    /// </summary>
    /// <param name="parentElement">The parent element. Use null if the element has not yet a parent.</param>
    /// <param name="identifier">the identifier of the element.</param>
    /// <param name="func">The service function.</param>
    /// <param name="format">The format of the element.</param>
    /// <param name="profiles">The profiles of the element.</param>
    /// <param name="uid">The unique identifier of the element.</param>
    /// <param name="isHidden">The hidden status of the element.</param>
    /// <param name="raiseTreeChanged">If true a treechanged event is raised; otherwise not</param>
    /// <param name="acquireLock">If true a write lock is acquired; otherwise not</param>
    /// <returns>The created element.</returns>
    IActionServiceElement CreateActionServiceElement(IBaseElement parentElement,
        string identifier,
        Action<IBaseElement, int?> func,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false,
        bool raiseTreeChanged = false,
        bool acquireLock = true);

    /// <summary>
    /// Creates a getter service element.
    /// </summary>
    /// <typeparam name="TOut">The return type of the service function.</typeparam>
    /// <param name="parentElement">The parent element. Use null if the element has not yet a parent.</param>
    /// <param name="identifier">the identifier of the element.</param>
    /// <param name="func">The service function.</param>
    /// <param name="format">The format of the element.</param>
    /// <param name="profiles">The profiles of the element.</param>
    /// <param name="uid">The unique identifier of the element.</param>
    /// <param name="isHidden">The hidden status of the element.</param>
    /// <param name="raiseTreeChanged">If true a treechanged event is raised; otherwise not</param>
    /// <param name="acquireLock">If true a write lock is acquired; otherwise not</param>
    /// <returns>The created element.</returns>
    IGetterServiceElement<TOut> CreateGetterServiceElement<TOut>(IBaseElement parentElement,
        string identifier,
        Func<IBaseElement, int?, TOut> func,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false,
        bool raiseTreeChanged = false,
        bool acquireLock = true);

    /// <summary>
    /// Creates a setter service element.
    /// </summary>
    /// <typeparam name="TIn">The parameter type of the service function.</typeparam>
    /// <param name="parentElement">The parent element. Use null if the element has not yet a parent.</param>
    /// <param name="identifier">the identifier of the element.</param>
    /// <param name="func">The service function.</param>
    /// <param name="format">The format of the element.</param>
    /// <param name="profiles">The profiles of the element.</param>
    /// <param name="uid">The unique identifier of the element.</param>
    /// <param name="isHidden">The hidden status of the element.</param>
    /// <param name="raiseTreeChanged">If true a treechanged event is raised; otherwise not</param>
    /// <param name="acquireLock">If true a write lock is acquired; otherwise not</param>
    /// <returns>The created element.</returns>
    ISetterServiceElement<TIn> CreateSetterServiceElement<TIn>(IBaseElement parentElement,
        string identifier,
        Action<IBaseElement, TIn, int?> func,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false,
        bool raiseTreeChanged = false,
        bool acquireLock = true);

    /// <summary>
    /// Creates a service element.
    /// </summary>
    /// <typeparam name="TIn">The parameter type of the service function.</typeparam>
    /// <typeparam name="TOut">The return type of the service function.</typeparam>
    /// <param name="parentElement">The parent element. Use null if the element has not yet a parent.</param>
    /// <param name="identifier">the identifier of the element.</param>
    /// <param name="func">The service function.</param>
    /// <param name="format">The format of the element.</param>
    /// <param name="profiles">The profiles of the element.</param>
    /// <param name="uid">The unique identifier of the element.</param>
    /// <param name="isHidden">The hidden status of the element.</param>
    /// <param name="raiseTreeChanged">If true a treechanged event is raised; otherwise not</param>
    /// <param name="acquireLock">If true a write lock is acquired; otherwise not</param>
    /// <returns>The created element.</returns>
    IServiceElement<TIn, TOut> CreateServiceElement<TIn, TOut>(IBaseElement parentElement,
        string identifier,
        Func<IBaseElement, TIn, int?, TOut> func,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false,
        bool raiseTreeChanged = false,
        bool acquireLock = true);

    /// <summary>
    /// Creates an event element.
    /// </summary>
    /// <param name="parentElement">The parent element. Use null if the element has not yet a parent.</param>
    /// <param name="identifier">the identifier of the element.</param>
    /// <param name="format">The format of the element.</param>
    /// <param name="profiles">The profiles of the element.</param>
    /// <param name="uid">The unique identifier of the element.</param>
    /// <param name="isHidden">The hidden status of the element.</param>
    /// <param name="raiseTreeChanged">If true a treechanged event is raised; otherwise not</param>
    /// <param name="acquireLock">If true a write lock is acquired; otherwise not</param>
    /// <returns>The created element.</returns>
    IEventElement CreateEventElement(IBaseElement parentElement,
        string identifier,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false,
        bool raiseTreeChanged = false,
        bool acquireLock = true);

    /// <summary>
    /// Creates a simple data element.
    /// A getdata and a setdata service child element are created, but no datachanged event element.
    /// </summary>
    /// <typeparam name="T">The type of the value represented by the element.</typeparam>
    /// <param name="parentElement">The parent element. Use null if the element has not yet a parent.</param>
    /// <param name="identifier">the identifier of the element.</param>
    /// <param name="value">The initial value of the element.</param>
    /// <param name="format">The format of the element.</param>
    /// <param name="profiles">The profiles of the element.</param>
    /// <param name="uid">The unique identifier of the element.</param>
    /// <param name="isHidden">The hidden status of the element.</param>
    /// <param name="raiseTreeChanged">If true a treechanged event is raised; otherwise not</param>
    /// <param name="acquireLock">If true a write lock is acquired; otherwise not</param>
    /// <returns>The created element.</returns>
    IReadWriteDataElement<T> CreateSimpleDataElement<T>(IBaseElement parentElement,
        string identifier,
        T value = default,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false,
        bool raiseTreeChanged = false,
        bool acquireLock = true);

    /// <summary>
    /// Creates a simple data element.
    /// A getdata and a setdata service child element are created, and optionally a datachanged event element.
    /// </summary>
    /// <typeparam name="T">The type of the value represented by the element.</typeparam>
    /// <param name="parentElement">The parent element. Use null if the element has not yet a parent.</param>
    /// <param name="identifier">the identifier of the element.</param>
    /// <param name="createDataChangedEventElement">If true a datachanged element is created; otherwise not.</param>
    /// <param name="value">The initial value of the element.</param>
    /// <param name="format">The format of the element.</param>
    /// <param name="profiles">The profiles of the element.</param>
    /// <param name="uid">The unique identifier of the element.</param>
    /// <param name="isHidden">The hidden status of the element.</param>
    /// <param name="raiseTreeChanged">If true a treechanged event is raised; otherwise not</param>
    /// <param name="acquireLock">If true a write lock is acquired; otherwise not</param>
    /// <returns>The created element.</returns>
    IReadWriteDataElement<T> CreateSimpleDataElement<T>(IBaseElement parentElement,
        string identifier,
        bool createDataChangedEventElement,
        T value = default,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false,
        bool raiseTreeChanged = false,
        bool acquireLock = true);

    /// <summary>
    /// Creates a simple data element.
    /// Optionally a getdata and a setdata service child element are created, but no datachanged event element.
    /// </summary>
    /// <typeparam name="T">The type of the value represented by the element.</typeparam>
    /// <param name="parentElement">The parent element. Use null if the element has not yet a parent.</param>
    /// <param name="identifier">the identifier of the element.</param>
    /// <param name="createGetDataServiceElement">If true a getdata service element is created; otherwise not.</param>
    /// <param name="createSetDataServiceElement">If true a setdata service element is created; otherwise not.</param>
    /// <param name="value">The initial value of the element.</param>
    /// <param name="format">The format of the element.</param>
    /// <param name="profiles">The profiles of the element.</param>
    /// <param name="uid">The unique identifier of the element.</param>
    /// <param name="isHidden">The hidden status of the element.</param>
    /// <param name="raiseTreeChanged">If true a treechanged event is raised; otherwise not</param>
    /// <param name="acquireLock">If true a write lock is acquired; otherwise not</param>
    /// <returns>The created element.</returns>
    IReadWriteDataElement<T> CreateSimpleDataElement<T>(IBaseElement parentElement,
        string identifier,
        bool createGetDataServiceElement,
        bool createSetDataServiceElement,
        T value = default,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false,
        bool raiseTreeChanged = false,
        bool acquireLock = true);

    /// <summary>
    /// Creates a simple data element.
    /// Optionally a getdata service, setdata service or datachanged event element are created.
    /// </summary>
    /// <typeparam name="T">The type of the value represented by the element.</typeparam>
    /// <param name="parentElement">The parent element. Use null if the element has not yet a parent.</param>
    /// <param name="identifier">the identifier of the element.</param>
    /// <param name="createGetDataServiceElement">If true a getdata service element is created; otherwise not.</param>
    /// <param name="createSetDataServiceElement">If true a setdata service element is created; otherwise not.</param>
    /// <param name="createDataChangedEventElement">If true a datachanged element is created; otherwise not.</param>
    /// <param name="value">The initial value of the element.</param>
    /// <param name="format">The format of the element.</param>
    /// <param name="profiles">The profiles of the element.</param>
    /// <param name="uid">The unique identifier of the element.</param>
    /// <param name="isHidden">The hidden status of the element.</param>
    /// <param name="raiseTreeChanged">If true a treechanged event is raised; otherwise not</param>
    /// <param name="acquireLock">If true a write lock is acquired; otherwise not</param>
    /// <returns>The created element.</returns>
    IReadWriteDataElement<T> CreateSimpleDataElement<T>(IBaseElement parentElement,
        string identifier,
        bool createGetDataServiceElement,
        bool createSetDataServiceElement,
        bool createDataChangedEventElement,
        T value = default,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false,
        bool raiseTreeChanged = false,
        bool acquireLock = true);

    /// <summary>
    /// Creates a readonly data element.
    /// </summary>
    /// <typeparam name="T">The type of the value represented by the element.</typeparam>
    /// <param name="parentElement">The parent element. Use null if the element has not yet a parent.</param>
    /// <param name="identifier">the identifier of the element.</param>
    /// <param name="getDataFunc">The getdata service function.</param>
    /// <param name="value">The initial value of the element.</param>
    /// <param name="cacheTimeout">The data cache timeout.</param>
    /// <param name="format">The format of the element.</param>
    /// <param name="profiles">The profiles of the element.</param>
    /// <param name="uid">The unique identifier of the element.</param>
    /// <param name="isHidden">The hidden status of the element.</param>
    /// <param name="raiseTreeChanged">If true a treechanged event is raised; otherwise not</param>
    /// <param name="acquireLock">If true a write lock is acquired; otherwise not</param>
    /// <returns>The created element.</returns>
    IReadDataElement<T> CreateReadOnlyDataElement<T>(IBaseElement parentElement,
        string identifier,
        Func<IBaseElement, T> getDataFunc,
        T value = default,
        TimeSpan? cacheTimeout = null,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false,
        bool raiseTreeChanged = false,
        bool acquireLock = true);

    /// <summary>
    /// Creates a readonly data element.
    /// </summary>
    /// <typeparam name="T">The type of the value represented by the element.</typeparam>
    /// <param name="parentElement">The parent element. Use null if the element has not yet a parent.</param>
    /// <param name="identifier">the identifier of the element.</param>
    /// <param name="getDataFunc">The getdata service function.</param>
    /// <param name="createDataChangedEventElement">If true a datachanged event element is created; otherwise not.</param>
    /// <param name="value">The initial value of the element.</param>
    /// <param name="cacheTimeout">The data cache timeout.</param>
    /// <param name="format">The format of the element.</param>
    /// <param name="profiles">The profiles of the element.</param>
    /// <param name="uid">The unique identifier of the element.</param>
    /// <param name="isHidden">The hidden status of the element.</param>
    /// <param name="raiseTreeChanged">If true a treechanged event is raised; otherwise not</param>
    /// <param name="acquireLock">If true a write lock is acquired; otherwise not</param>
    /// <returns>The created element.</returns>
    IReadDataElement<T> CreateReadOnlyDataElement<T>(IBaseElement parentElement,
        string identifier,
        Func<IBaseElement, T> getDataFunc,
        bool createDataChangedEventElement,
        T value = default,
        TimeSpan? cacheTimeout = null,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false,
        bool raiseTreeChanged = false,
        bool acquireLock = true);

    /// <summary>
    /// Creates a writeonly data element.
    /// </summary>
    /// <typeparam name="T">The type of the value represented by the element.</typeparam>
    /// <param name="parentElement">The parent element. Use null if the element has not yet a parent.</param>
    /// <param name="identifier">the identifier of the element.</param>
    /// <param name="setDataFunc">The setdata service function.</param>
    /// <param name="format">The format of the element.</param>
    /// <param name="profiles">The profiles of the element.</param>
    /// <param name="uid">The unique identifier of the element.</param>
    /// <param name="isHidden">The hidden status of the element.</param>
    /// <param name="raiseTreeChanged">If true a treechanged event is raised; otherwise not</param>
    /// <param name="acquireLock">If true a write lock is acquired; otherwise not</param>
    /// <returns>The created element.</returns>
    IWriteDataElement<T> CreateWriteOnlyDataElement<T>(IBaseElement parentElement,
        string identifier,
        Action<IBaseElement, T> setDataFunc,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false,
        bool raiseTreeChanged = false,
        bool acquireLock = true);

    /// <summary>
    /// Creates a writeonly data element.
    /// </summary>
    /// <typeparam name="T">The type of the value represented by the element.</typeparam>
    /// <param name="parentElement">The parent element. Use null if the element has not yet a parent.</param>
    /// <param name="identifier">the identifier of the element.</param>
    /// <param name="setDataFunc">The setdata service function.</param>
    /// <param name="createDataChangedEventElement">If true a datachanged event element is created; otherwise not.</param>
    /// <param name="format">The format of the element.</param>
    /// <param name="profiles">The profiles of the element.</param>
    /// <param name="uid">The unique identifier of the element.</param>
    /// <param name="isHidden">The hidden status of the element.</param>
    /// <param name="raiseTreeChanged">If true a treechanged event is raised; otherwise not</param>
    /// <param name="acquireLock">If true a write lock is acquired; otherwise not</param>
    /// <returns>The created element.</returns>
    IWriteDataElement<T> CreateWriteOnlyDataElement<T>(IBaseElement parentElement,
        string identifier,
        Action<IBaseElement, T> setDataFunc,
        bool createDataChangedEventElement,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false,
        bool raiseTreeChanged = false,
        bool acquireLock = true);

    /// <summary>
    /// Creates a data element.
    /// </summary>
    /// <typeparam name="T">The type of the value represented by the element.</typeparam>
    /// <param name="parentElement">The parent element. Use null if the element has not yet a parent.</param>
    /// <param name="identifier">the identifier of the element.</param>
    /// <param name="getDataFunc">The getdata service function.</param>
    /// <param name="setDataFunc">The setdata service function.</param>
    /// <param name="value">The initial value of the element.</param>
    /// <param name="cacheTimeout">The data cache timeout.</param>
    /// <param name="format">The format of the element.</param>
    /// <param name="profiles">The profiles of the element.</param>
    /// <param name="uid">The unique identifier of the element.</param>
    /// <param name="isHidden">The hidden status of the element.</param>
    /// <param name="raiseTreeChanged">If true a treechanged event is raised; otherwise not</param>
    /// <param name="acquireLock">If true a write lock is acquired; otherwise not</param>
    /// <returns>The created element.</returns>
    IReadWriteDataElement<T> CreateDataElement<T>(IBaseElement parentElement,
        string identifier,
        Func<IBaseElement, T> getDataFunc,
        Action<IBaseElement, T> setDataFunc,
        T value = default,
        TimeSpan? cacheTimeout = null,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false,
        bool raiseTreeChanged = false,
        bool acquireLock = true);

    /// <summary>
    /// Creates a data element.
    /// </summary>
    /// <typeparam name="T">The type of the value represented by the element.</typeparam>
    /// <param name="parentElement">The parent element. Use null if the element has not yet a parent.</param>
    /// <param name="identifier">the identifier of the element.</param>
    /// <param name="getDataFunc">The getdata service function.</param>
    /// <param name="setDataFunc">The setdata service function.</param>
    /// <param name="createDataChangedEventElement">If true a datachanged event element is created; otherwise not.</param>
    /// <param name="value">The initial value of the element.</param>
    /// <param name="cacheTimeout">The data cache timeout.</param>
    /// <param name="format">The format of the element.</param>
    /// <param name="profiles">The profiles of the element.</param>
    /// <param name="uid">The unique identifier of the element.</param>
    /// <param name="isHidden">The hidden status of the element.</param>
    /// <param name="raiseTreeChanged">If true a treechanged event is raised; otherwise not</param>
    /// <param name="acquireLock">If true a write lock is acquired; otherwise not</param>
    /// <returns>The created element.</returns>
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
        bool isHidden = false,
        bool raiseTreeChanged = false,
        bool acquireLock = true);

    /// <summary>
    /// Adds a child element to an element.
    /// </summary>
    /// <param name="parentElement">The parent element.</param>
    /// <param name="element">The child element to add.</param>
    /// <param name="raiseTreeChanged">If true a treechanged event is raised; otherwise not.</param>
    /// <param name="acquireLock">If true a write lock is acquired; otherwise not.</param>
    void AddElement(IBaseElement parentElement,
        IBaseElement element,
        bool raiseTreeChanged = false,
        bool acquireLock = true);

    /// <summary>
    /// Removes a child element from an element.
    /// </summary>
    /// <param name="parentElement">The parent element.</param>
    /// <param name="element">The child element to remove.</param>
    /// <param name="raiseTreeChanged">If true a treechanged event is raised; otherwise not.</param>
    /// <param name="acquireLock">If true a write lock is acquired; otherwise not.</param>
    void RemoveElement(IBaseElement parentElement,
        IBaseElement element,
        bool raiseTreeChanged = false,
        bool acquireLock = true);

    /// <summary>
    /// Adds a link to an element.
    /// </summary>
    /// <param name="sourceElement">The source element of the link.</param>
    /// <param name="targetElement">The target element of the link.</param>
    /// <param name="identifier">The identifier of the link; if null the identifier of the target element is used.</param>
    /// <param name="raiseTreeChanged">If true a treechanged event is raised; otherwise not.</param>
    /// <param name="acquireLock">If true a write lock is acquired; otherwise not.</param>
    void AddLink(IBaseElement sourceElement,
        IBaseElement targetElement,
        string identifier = null,
        bool raiseTreeChanged = false,
        bool acquireLock = true);

    /// <summary>
    /// Removes a link to an element.
    /// </summary>
    /// <param name="sourceElement">The source element of the link.</param>
    /// <param name="targetElement">The target element of the link.</param>
    /// <param name="raiseTreeChanged">If true a treechanged event is raised; otherwise not.</param>
    /// <param name="acquireLock">If true a write lock is acquired; otherwise not.</param>
    void RemoveLink(IBaseElement sourceElement,
        IBaseElement targetElement,
        bool raiseTreeChanged = false,
        bool acquireLock = true);

    /// <summary>
    /// Removes a link to an element.
    /// </summary>
    /// <param name="sourceElement">The source element of the link.</param>
    /// <param name="identifier">The identifier of the link.</param>
    /// <param name="raiseTreeChanged">If true a treechanged event is raised; otherwise not.</param>
    /// <param name="acquireLock">If true a write lock is acquired; otherwise not.</param>
    void RemoveLink(IBaseElement sourceElement,
        string identifier,
        bool raiseTreeChanged = false,
        bool acquireLock = true);
}