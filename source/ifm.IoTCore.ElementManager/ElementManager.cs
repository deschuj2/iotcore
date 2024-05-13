namespace ifm.IoTCore.ElementManager;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Common;
using Common.Exceptions;
using Contracts;
using Contracts.Elements;
using Contracts.Elements.Formats;
using Contracts.Elements.ServiceData.Requests;
using Contracts.Elements.ServiceData.Responses;
using Contracts.Elements.Tree;
using Elements;
using EventSender.Contracts;
using PersistenceManager.Contracts;

public class ElementManager : IElementManager
{
    private readonly ElementCache _elementCache;
    private readonly IPersistenceManager _persistenceManager;
    private readonly SubscriptionManager _subscriptionManager;
    private readonly ReaderWriterLockSlim _lock = new();
    private readonly int _timeout;

    public ElementManager(ElementCache elementCache, 
        IPersistenceManager persistenceManager, 
        IEventSender eventSender,
        int timeout = 10000)
    {
        _elementCache = elementCache ?? throw new ArgumentNullException(nameof(elementCache));
        _persistenceManager = persistenceManager ?? throw new ArgumentNullException(nameof(persistenceManager));
        _timeout = timeout;

        _subscriptionManager = new SubscriptionManager(eventSender);
    }

    public bool IsReadLockHeld => _lock.IsReadLockHeld;
    
    public bool IsWriteLockHeld => _lock.IsWriteLockHeld;

    public void EnterReadLock()
    {
        if (!_lock.TryEnterReadLock(_timeout))
        {
            throw new LockedException("The tree manager is locked");
        }
    }

    public void ExitReadLock()
    {
        _lock.ExitReadLock();
    }

    public void EnterWriteLock()
    {
        if (!_lock.TryEnterWriteLock(_timeout))
        {
            throw new LockedException("The tree manager is locked");
        }
    }

    public void ExitWriteLock()
    {
        _lock.ExitWriteLock();
    }

    public IBaseElement GetElementByAddress(string address)
    {
        return _elementCache.GetElementByAddress(address);
    }

    public IBaseElement Root 
    { 
        get => _root;
        set => _elementCache.Root = _root = value;
    }
    private IBaseElement _root;

    // ToDo: Check identifiers for reserved identifiers from Identifiers

    public IBaseElement CreateElement(IBaseElement parentElement,
        string type,
        string identifier,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false,
        bool raiseTreeChanged = false,
        bool acquireLock = true)
    {
        if (acquireLock) EnterWriteLock();
        try
        {
            var address = CreateAddress(parentElement?.Address, identifier);
            var element = new BaseElement(type,
                identifier,
                address,
                format,
                profiles,
                uid,
                isHidden);

            if (parentElement == null) return element;
            InnerCreateChild((BaseElement)parentElement, element);

            if (raiseTreeChanged)
            {
                ((BaseElement)parentElement).RaiseTreeChanged(new TreeChangedEventArgs<IBaseElement>(TreeChangedActions.ChildAdded, parentElement, element));
            }

            return element;
        }
        finally
        {
            if (acquireLock) ExitWriteLock();
        }
    }

    public IStructureElement CreateStructureElement(IBaseElement parentElement,
        string identifier,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false,
        bool raiseTreeChanged = false,
        bool acquireLock = true)
    {
        if (acquireLock) EnterWriteLock();
        try
        {
            var address = CreateAddress(parentElement?.Address, identifier);
            var element = new StructureElement(identifier, 
                address, 
                format, 
                profiles, 
                uid, 
                isHidden);

            if (parentElement == null) return element;
            InnerCreateChild((BaseElement)parentElement, element);

            if (raiseTreeChanged)
            {
                ((BaseElement)parentElement).RaiseTreeChanged(new TreeChangedEventArgs<IBaseElement>(TreeChangedActions.ChildAdded, parentElement, element));
            }

            return element;
        }
        finally
        {
            if (acquireLock) ExitWriteLock();
        }
    }

    public IActionServiceElement CreateActionServiceElement(IBaseElement parentElement,
        string identifier,
        Action<IBaseElement, int?> func,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false,
        bool raiseTreeChanged = false,
        bool acquireLock = true)
    {
        if (acquireLock) EnterWriteLock();
        try
        {
            var address = CreateAddress(parentElement?.Address, identifier);
            var element = new ActionServiceElement(identifier, 
                address, 
                func, 
                format, 
                profiles, 
                uid, 
                isHidden);

            if (parentElement == null) return element;
            InnerCreateChild((BaseElement)parentElement, element);

            if (raiseTreeChanged)
            {
                ((BaseElement)parentElement).RaiseTreeChanged(new TreeChangedEventArgs<IBaseElement>(TreeChangedActions.ChildAdded, parentElement, element));
            }

            return element;
        }
        finally
        {
            if (acquireLock) ExitWriteLock();
        }
    }

    public IGetterServiceElement<TOut> CreateGetterServiceElement<TOut>(IBaseElement parentElement,
        string identifier,
        Func<IBaseElement, int?, TOut> func,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false,
        bool raiseTreeChanged = false,
        bool acquireLock = true)
    {
        if (acquireLock) EnterWriteLock();
        try
        {
            var address = CreateAddress(parentElement?.Address, identifier);
            var element = new GetterServiceElement<TOut>(identifier, 
                address, 
                func, 
                format, 
                profiles, 
                uid, 
                isHidden);

            if (parentElement == null) return element;
            InnerCreateChild((BaseElement)parentElement, element);

            if (raiseTreeChanged)
            {
                ((BaseElement)parentElement).RaiseTreeChanged(new TreeChangedEventArgs<IBaseElement>(TreeChangedActions.ChildAdded, parentElement, element));
            }

            return element;
        }
        finally
        {
            if (acquireLock) ExitWriteLock();
        }
    }

    public ISetterServiceElement<TIn> CreateSetterServiceElement<TIn>(IBaseElement parentElement,
        string identifier,
        Action<IBaseElement, TIn, int?> func,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false,
        bool raiseTreeChanged = false,
        bool acquireLock = true)
    {
        if (acquireLock) EnterWriteLock();
        try
        {
            var address = CreateAddress(parentElement?.Address, identifier);
            var element = new SetterServiceElement<TIn>(identifier, 
                address, 
                func, 
                format, 
                profiles, 
                uid, 
                isHidden);

            if (parentElement == null) return element;
            InnerCreateChild((BaseElement)parentElement, element);

            if (raiseTreeChanged)
            {
                ((BaseElement)parentElement).RaiseTreeChanged(new TreeChangedEventArgs<IBaseElement>(TreeChangedActions.ChildAdded, parentElement, element));
            }

            return element;
        }
        finally
        {
            if (acquireLock) ExitWriteLock();
        }
    }

    public IServiceElement<TIn, TOut> CreateServiceElement<TIn, TOut>(IBaseElement parentElement,
        string identifier,
        Func<IBaseElement, TIn, int?, TOut> func,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false,
        bool raiseTreeChanged = false,
        bool acquireLock = true)
    {
        if (acquireLock) EnterWriteLock();
        try
        {
            var address = CreateAddress(parentElement?.Address, identifier);
            var element = new ServiceElement<TIn, TOut>(identifier, 
                address, 
                func, 
                format, 
                profiles, 
                uid, 
                isHidden);

            if (parentElement == null) return element;
            InnerCreateChild((BaseElement)parentElement, element);

            if (raiseTreeChanged)
            {
                ((BaseElement)parentElement).RaiseTreeChanged(new TreeChangedEventArgs<IBaseElement>(TreeChangedActions.ChildAdded, parentElement, element));
            }

            return element;
        }
        finally
        {
            if (acquireLock) ExitWriteLock();
        }
    }

    public IEventElement CreateEventElement(IBaseElement parentElement,
        string identifier, 
        Format format = null, 
        IEnumerable<string> profiles = null, 
        string uid = null, 
        bool isHidden = false,
        bool raiseTreeChanged = false,
        bool acquireLock = true)
    {
        if (acquireLock) EnterWriteLock();
        try
        {
            var address = CreateAddress(parentElement?.Address, identifier);
            var element = new EventElement(identifier, 
                address, 
                _persistenceManager, 
                format, 
                profiles, 
                uid, 
                isHidden);
            CreateServiceElement<SubscribeRequestServiceData, SubscribeResponseServiceData>(element, Identifiers.Subscribe, element.SubscribeFunc, acquireLock: false);
            CreateSetterServiceElement<UnsubscribeRequestServiceData>(element, Identifiers.Unsubscribe, element.UnsubscribeFunc, acquireLock: false);
            element.EventRaised += (sender, _) =>
            {
                var eventElement = (IEventElement)sender;
                var subscriptions = eventElement.GetSubscriptions();
                _subscriptionManager.SendEvents((IEventElement)sender, subscriptions.ToList());
            };

            if (parentElement == null) return element;
            InnerCreateChild((BaseElement)parentElement, element);

            if (raiseTreeChanged)
            {
                ((BaseElement)parentElement).RaiseTreeChanged(new TreeChangedEventArgs<IBaseElement>(TreeChangedActions.ChildAdded, parentElement, element));
            }

            return element;
        }
        finally
        {
            if (acquireLock) ExitWriteLock();
        }
    }

    public IReadWriteDataElement<T> CreateSimpleDataElement<T>(IBaseElement parentElement,
        string identifier,
        T value = default,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false,
        bool raiseTreeChanged = false,
        bool acquireLock = true)
    {
        return CreateSimpleDataElement(parentElement,
            identifier,
            true,
            true,
            false,
            value,
            format,
            profiles,
            uid,
            isHidden,
            raiseTreeChanged,
            acquireLock);
    }

    public IReadWriteDataElement<T> CreateSimpleDataElement<T>(IBaseElement parentElement, 
        string identifier,
        bool createDataChangedEventElement, 
        T value = default, 
        Format format = null, 
        IEnumerable<string> profiles = null,
        string uid = null, 
        bool isHidden = false, 
        bool raiseTreeChanged = false, 
        bool acquireLock = true)
    {
        return CreateSimpleDataElement(parentElement,
            identifier,
            true,
            true,
            createDataChangedEventElement,
            value,
            format,
            profiles,
            uid,
            isHidden,
            raiseTreeChanged,
            acquireLock);
    }

    public IReadWriteDataElement<T> CreateSimpleDataElement<T>(IBaseElement parentElement,
        string identifier,
        bool createGetDataServiceElement,
        bool createSetDataServiceElement,
        T value = default,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false,
        bool raiseTreeChanged = false,
        bool acquireLock = true)
    {
        return CreateSimpleDataElement(parentElement,
            identifier,
            createGetDataServiceElement,
            createSetDataServiceElement,
            false,
            value,
            format,
            profiles,
            uid,
            isHidden,
            raiseTreeChanged,
            acquireLock);
    }

    public IReadWriteDataElement<T> CreateSimpleDataElement<T>(IBaseElement parentElement,
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
        bool acquireLock = true)
    {
        if (acquireLock) EnterWriteLock();
        try
        {
            var address = CreateAddress(parentElement?.Address, identifier);
            var element = new SimpleDataElement<T>(identifier, 
                address, 
                value, 
                format, 
                profiles, 
                uid, 
                isHidden);

            if (createGetDataServiceElement) CreateGetterServiceElement(element, Identifiers.GetData, element.GetDataFunc, acquireLock: false);
            if (createSetDataServiceElement) CreateSetterServiceElement<SetDataRequestServiceData>(element, Identifiers.SetData, element.SetDataFunc, acquireLock: false);
            if (createDataChangedEventElement)
            {
                element.DataChangedEventElement = CreateEventElement(element, Identifiers.DataChanged, acquireLock: false);
            }

            if (parentElement == null) return element;
            InnerCreateChild((BaseElement)parentElement, element);

            if (raiseTreeChanged)
            {
                ((BaseElement)parentElement).RaiseTreeChanged(new TreeChangedEventArgs<IBaseElement>(TreeChangedActions.ChildAdded, parentElement, element));
            }

            return element;
        }
        finally
        {
            if (acquireLock) ExitWriteLock();
        }
    }

    public IReadDataElement<T> CreateReadOnlyDataElement<T>(IBaseElement parentElement,
        string identifier,
        Func<IBaseElement, T> getDataFunc,
        T value = default,
        TimeSpan? cacheTimeout = null,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false,
        bool raiseTreeChanged = false,
        bool acquireLock = true)
    {
        return CreateReadOnlyDataElement(parentElement,
            identifier,
            getDataFunc,
            false,
            value,
            cacheTimeout,
            format,
            profiles,
            uid,
            isHidden,
            raiseTreeChanged,
            acquireLock);
    }

    public IReadDataElement<T> CreateReadOnlyDataElement<T>(IBaseElement parentElement,
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
        bool acquireLock = true)
    {
        if (acquireLock) EnterWriteLock();
        try
        {
            var address = CreateAddress(parentElement?.Address, identifier);
            var element = new ReadOnlyDataElement<T>(identifier, 
                address, 
                getDataFunc, 
                value, 
                cacheTimeout, 
                format, 
                profiles, 
                uid, 
                isHidden);

            CreateGetterServiceElement(element, Identifiers.GetData, element.GetDataFunc, acquireLock: false);
            if (createDataChangedEventElement)
            {
                element.DataChangedEventElement = CreateEventElement(element, Identifiers.DataChanged, acquireLock: false);
            }

            if (parentElement == null) return element;
            InnerCreateChild((BaseElement)parentElement, element);

            if (raiseTreeChanged)
            {
                ((BaseElement)parentElement).RaiseTreeChanged(new TreeChangedEventArgs<IBaseElement>(TreeChangedActions.ChildAdded, parentElement, element));
            }

            return element;
        }
        finally
        {
            if (acquireLock) ExitWriteLock();
        }
    }

    public IWriteDataElement<T> CreateWriteOnlyDataElement<T>(IBaseElement parentElement,
        string identifier,
        Action<IBaseElement, T> setDataFunc,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false,
        bool raiseTreeChanged = false,
        bool acquireLock = true)
    {
        return CreateWriteOnlyDataElement(parentElement,
            identifier,
            setDataFunc,
            false,
            format,
            profiles,
            uid,
            isHidden,
            raiseTreeChanged,
            acquireLock);
    }

    public IWriteDataElement<T> CreateWriteOnlyDataElement<T>(IBaseElement parentElement,
        string identifier,
        Action<IBaseElement, T> setDataFunc,
        bool createDataChangedEventElement,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false,
        bool raiseTreeChanged = false,
        bool acquireLock = true)
    {
        if (acquireLock) EnterWriteLock();
        try
        {
            var address = CreateAddress(parentElement?.Address, identifier);
            var element = new WriteOnlyDataElement<T>(identifier, 
                address, 
                setDataFunc, 
                default, 
                format, 
                profiles, 
                uid, 
                isHidden);

            CreateSetterServiceElement<SetDataRequestServiceData>(element, Identifiers.SetData, element.SetDataFunc, acquireLock: false);
            if (createDataChangedEventElement)
            {
                element.DataChangedEventElement = CreateEventElement(element, Identifiers.DataChanged, acquireLock: false);
            }

            if (parentElement == null) return element;
            InnerCreateChild((BaseElement)parentElement, element);

            if (raiseTreeChanged)
            {
                ((BaseElement)parentElement).RaiseTreeChanged(new TreeChangedEventArgs<IBaseElement>(TreeChangedActions.ChildAdded, parentElement, element));
            }
            return element;
        }
        finally
        {
            if (acquireLock) ExitWriteLock();
        }
    }

    public IReadWriteDataElement<T> CreateDataElement<T>(IBaseElement parentElement,
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
        bool acquireLock = true)
    {
        return CreateDataElement(parentElement,
            identifier,
            getDataFunc,
            setDataFunc,
            false,
            value,
            cacheTimeout,
            format,
            profiles,
            uid,
            isHidden,
            raiseTreeChanged,
            acquireLock);
    }

    public IReadWriteDataElement<T> CreateDataElement<T>(IBaseElement parentElement,
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
        bool acquireLock = true)
    {
        if (acquireLock) EnterWriteLock();
        try
        {
            var address = CreateAddress(parentElement?.Address, identifier);
            var element = new DataElement<T>(identifier, 
                address, 
                getDataFunc, 
                setDataFunc, 
                value, 
                cacheTimeout, 
                format, 
                profiles, 
                uid, 
                isHidden);

            CreateGetterServiceElement(element, Identifiers.GetData, element.GetDataFunc, acquireLock: false);
            CreateSetterServiceElement<SetDataRequestServiceData>(element, Identifiers.SetData, element.SetDataFunc, acquireLock: false);
            if (createDataChangedEventElement)
            {
                element.DataChangedEventElement = CreateEventElement(element, Identifiers.DataChanged, acquireLock: false);
            }

            if (parentElement == null) return element;
            InnerCreateChild((BaseElement)parentElement, element);

            if (raiseTreeChanged)
            {
                ((BaseElement)parentElement).RaiseTreeChanged(new TreeChangedEventArgs<IBaseElement>(TreeChangedActions.ChildAdded, parentElement, element));
            }

            return element;
        }
        finally
        {
            if (acquireLock) ExitWriteLock();
        }
    }

    public void AddElement(IBaseElement parentElement, 
        IBaseElement element, 
        bool raiseTreeChanged = false,
        bool acquireLock = true)
    {
        if (parentElement == null) throw new ArgumentNullException(nameof(parentElement));
        if (element == null) throw new ArgumentNullException(nameof(element));

        if (acquireLock) EnterWriteLock();
        try
        {
            InnerAddChild((BaseElement)parentElement, (BaseElement)element);
            if (raiseTreeChanged)
            {
                ((BaseElement)parentElement).RaiseTreeChanged(new TreeChangedEventArgs<IBaseElement>(TreeChangedActions.ChildAdded, parentElement, element));
            }
        }
        finally
        {
            if (acquireLock) ExitWriteLock();
        }
    }

    public void RemoveElement(IBaseElement parentElement, 
        IBaseElement element, 
        bool raiseTreeChanged = false, 
        bool acquireLock = true)
    {
        if (parentElement == null) throw new ArgumentNullException(nameof(parentElement));
        if (element == null) throw new ArgumentNullException(nameof(element));

        if (acquireLock) EnterWriteLock();
        try
        {
            InnerRemoveChild((BaseElement)parentElement, (BaseElement)element);
            if (raiseTreeChanged)
            {
                ((BaseElement)parentElement).RaiseTreeChanged(new TreeChangedEventArgs<IBaseElement>(TreeChangedActions.ChildRemoved, parentElement, element));
            }
        }
        finally
        {
            if (acquireLock) ExitWriteLock();
        }
    }

    public void AddLink(IBaseElement sourceElement, 
        IBaseElement targetElement, 
        string identifier = null, 
        bool raiseTreeChanged = false, 
        bool acquireLock = true)
    {
        if (sourceElement == null) throw new ArgumentNullException(nameof(sourceElement));
        if (targetElement == null) throw new ArgumentNullException(nameof(targetElement));

        if (acquireLock) EnterWriteLock();
        try
        {
            identifier ??= targetElement.Identifier;
            InnerAddLink((BaseElement)sourceElement, (BaseElement)targetElement, identifier);
            if (raiseTreeChanged)
            {
                ((BaseElement)sourceElement).RaiseTreeChanged(new TreeChangedEventArgs<IBaseElement>(TreeChangedActions.LinkAdded, sourceElement, targetElement, identifier));
            }
        }
        finally
        {
            if (acquireLock) ExitWriteLock();
        }
    }

    public void RemoveLink(IBaseElement sourceElement, 
        IBaseElement targetElement, 
        bool raiseTreeChanged = false, 
        bool acquireLock = true)
    {
        if (sourceElement == null) throw new ArgumentNullException(nameof(sourceElement));
        if (targetElement == null) throw new ArgumentNullException(nameof(targetElement));

        if (acquireLock) EnterWriteLock();
        try
        {
            var identifier = InnerRemoveLink((BaseElement)sourceElement, (BaseElement)targetElement);
            if (raiseTreeChanged)
            {
                ((BaseElement)sourceElement).RaiseTreeChanged(new TreeChangedEventArgs<IBaseElement>(TreeChangedActions.LinkRemoved, sourceElement, targetElement, identifier));
            }
        }
        finally
        {
            if (acquireLock) ExitWriteLock();
        }
    }

    public void RemoveLink(IBaseElement sourceElement,
        string identifier,
        bool raiseTreeChanged = false,
        bool acquireLock = true)
    {
        if (sourceElement == null) throw new ArgumentNullException(nameof(sourceElement));
        if (identifier == null) throw new ArgumentNullException(nameof(identifier));

        if (acquireLock) EnterWriteLock();
        try
        {
            var targetElement = InnerRemoveLink((BaseElement)sourceElement, identifier);
            if (raiseTreeChanged)
            {
                ((BaseElement)sourceElement).RaiseTreeChanged(new TreeChangedEventArgs<IBaseElement>(TreeChangedActions.LinkRemoved, sourceElement, targetElement, identifier));
            }
        }
        finally
        {
            if (acquireLock) ExitWriteLock();
        }
    }

    public void RaiseTreeChanged(TreeChangedActions action, IBaseElement sourceElement, IBaseElement targetElement, string identifier = null)
    {
        if (sourceElement == null) throw new ArgumentNullException(nameof(sourceElement));

        ((BaseElement)sourceElement).RaiseTreeChanged(new TreeChangedEventArgs<IBaseElement>(action, sourceElement, targetElement));
    }

    public IWriteTreeTransaction CreateWriteTreeTransaction()
    {
        return new WriteTreeTransaction(this);
    }

    public IReadTreeTransaction CreateReadTreeTransaction()
    {
        return new ReadTreeTransaction(this);
    }

    private static void InnerCreateChild(BaseElement parentElement, BaseElement childElement)
    {
        // Check if source node has reference to node with same identifier
        if (parentElement.References.ForwardReferences?.FirstOrDefault(x => string.Equals(x.Identifier, childElement.Identifier, StringComparison.OrdinalIgnoreCase)) != null)
        {
            throw new AlreadyExistsException($"The node '{parentElement}' already has a reference to node '{childElement}'");
        }

        // Wire up nodes
        parentElement.EnterWriteLock();
        try
        {
            childElement.EnterWriteLock();
            try
            {
                childElement.Parent = parentElement;
                childElement.References.AddInverseReference(childElement.Identifier, parentElement, childElement, ReferenceTypes.Child);
                childElement.TreeChanged += parentElement.OnTreeChanged;
            }
            finally
            {
                childElement.ExitWriteLock();
            }
            parentElement.References.AddForwardReference(childElement.Identifier, parentElement, childElement, ReferenceTypes.Child);
        }
        finally
        {
            parentElement.ExitWriteLock();
        }
    }

    private static void InnerAddChild(BaseElement parentElement, BaseElement childElement)
    {
        // Check if source node has reference to target node or node with same identifier already exists
        if (parentElement.References.ForwardReferences?.FirstOrDefault(x => x.TargetNode == childElement) != null ||
            parentElement.References.ForwardReferences?.FirstOrDefault(x => string.Equals(x.Identifier, childElement.Identifier, StringComparison.OrdinalIgnoreCase)) != null)
        {
            throw new AlreadyExistsException($"The node '{parentElement}' already has a reference to node '{childElement}'");
        }

        // Check target node or any node down the target node tree references the source node (circular dependency)
        if (IsCircularDependency(parentElement, childElement))
        {
            throw new BadRequestException($"Adding node '{childElement}' to node '{parentElement}' creates a circular dependency.");
        }

        // Check if target node already has a parent
        if (childElement.Parent != null)
        {
            throw new BadRequestException($"The node {childElement} already has a parent node {childElement.Parent}");
        }

        // Wire up nodes
        parentElement.EnterWriteLock();
        try
        {
            childElement.EnterWriteLock();
            try
            {
                childElement.Parent = parentElement;
                childElement.References.AddInverseReference(childElement.Identifier, parentElement, childElement, ReferenceTypes.Child);
                PatchChildAddress(parentElement, childElement);
                childElement.TreeChanged += parentElement.OnTreeChanged;
            }
            finally
            {
                childElement.ExitWriteLock();
            }
            parentElement.References.AddForwardReference(childElement.Identifier, parentElement, childElement, ReferenceTypes.Child);
        }
        finally
        {
            parentElement.ExitWriteLock();
        }
    }

    private static void PatchChildAddress(BaseElement parentElement, BaseElement childElement)
    {
        childElement.Address = CreateAddress(parentElement.Address, childElement.Identifier);
        if (childElement.References.ForwardReferences == null) return;
        foreach (var item in childElement.References.ForwardReferences)
        {
            if (item.IsChild) PatchChildAddress(childElement, item.TargetNode);
        }
    }

    private static void InnerRemoveChild(BaseElement parentElement, BaseElement childElement)
    {
        // Check if source node has a reference to target node and that it is a child node
        if (parentElement.References.ForwardReferences?.FirstOrDefault(x => x.TargetNode == childElement && x.IsChild) == null)
        {
            throw new NotFoundException($"The node {childElement} is not a child node of {parentElement}");
        }

        // Check if the target node or any node down the target node tree is linked
        if (IsLinked(childElement))
        {
            throw new BadRequestException($"The node {childElement} is linked or contains linked nodes");
        }

        // Wire up nodes
        parentElement.EnterWriteLock();
        try
        {
            childElement.EnterWriteLock();
            try
            {
                childElement.Parent = null;
                childElement.References.RemoveInverseReference(parentElement, childElement);
                childElement.TreeChanged -= parentElement.OnTreeChanged;
            }
            finally
            {
                childElement.ExitWriteLock();
            }
            parentElement.References.RemoveForwardReference(parentElement, childElement);
        }
        finally
        {
            parentElement.ExitWriteLock();
        }
    }

    private static void InnerAddLink(BaseElement sourceElement, BaseElement targetElement, string identifier)
    {
        // Check if source node has reference to target node or node with same identifier already exists
        if (sourceElement.References.ForwardReferences?.FirstOrDefault(x => x.TargetNode == targetElement) != null ||
            sourceElement.References.ForwardReferences?.FirstOrDefault(x => string.Equals(x.Identifier, identifier, StringComparison.OrdinalIgnoreCase)) != null)
        {
            throw new AlreadyExistsException($"The node '{sourceElement}' already has a reference to node '{targetElement}' or a reference with identifier {identifier}");
        }

        // Check target node or any node down the target node tree references the source node (circular dependency)
        if (IsCircularDependency(sourceElement, targetElement))
        {
            throw new BadRequestException($"Adding node '{targetElement}' to node '{sourceElement}' creates a circular dependency.");
        }

        // Check if target node has no parent
        //if (targetElement.Parent == null)
        //{
        //    throw new BadRequestException($"The node {targetElement} has no parent node");
        //}

        // Wire up nodes
        sourceElement.EnterWriteLock();
        try
        {
            targetElement.EnterWriteLock();
            try
            {
                targetElement.References.AddInverseReference(identifier, sourceElement, targetElement, ReferenceTypes.Link);
            }
            finally
            {
                targetElement.ExitWriteLock();
            }
            sourceElement.References.AddForwardReference(identifier, sourceElement, targetElement, ReferenceTypes.Link);
        }
        finally
        {
            sourceElement.ExitWriteLock();
        }
    }

    private static string InnerRemoveLink(BaseElement sourceElement, BaseElement targetElement)
    {
        // Wire up nodes
        sourceElement.EnterWriteLock();
        try
        {
            var reference = sourceElement.References.ForwardReferences?.FirstOrDefault(x => x.TargetNode == targetElement && x.IsLink) ?? 
                            throw new NotFoundException($"The node {sourceElement} does not have a link to {targetElement}");
            var identifier = reference.Identifier;

            targetElement.EnterWriteLock();
            try
            {
                targetElement.References.RemoveInverseReference(sourceElement, targetElement);
            }
            finally
            {
                targetElement.ExitWriteLock();
            }
            sourceElement.References.RemoveForwardReference(sourceElement, targetElement);

            return identifier;
        }
        finally
        {
            sourceElement.ExitWriteLock();
        }
    }

    private static BaseElement InnerRemoveLink(BaseElement sourceElement, string identifier)
    {
        // Wire up nodes
        sourceElement.EnterWriteLock();
        try
        {
            var reference = sourceElement.References.ForwardReferences?.FirstOrDefault(x => x.Identifier == identifier && x.IsLink) ?? 
                                throw new NotFoundException($"The node {sourceElement} does not have a link {identifier}");
            var targetElement = reference.TargetNode;

            targetElement.EnterWriteLock();
            try
            {
                targetElement.References.RemoveInverseReference(sourceElement, targetElement);
            }
            finally
            {
                targetElement.ExitWriteLock();
            }
            sourceElement.References.RemoveForwardReference(sourceElement, targetElement);

            return targetElement;
        }
        finally
        {
            sourceElement.ExitWriteLock();
        }
    }

    private static bool IsCircularDependency(BaseElement sourceElement, BaseElement targetElement)
    {
        if (sourceElement == targetElement) return true;
        return targetElement.References.ForwardReferences != null && targetElement.References.ForwardReferences.Any(x => IsCircularDependency(sourceElement, x.TargetNode));
    }

    private static bool IsLinked(BaseElement targetElement)
    {
        if (targetElement.References.InverseReferences.FirstOrDefault(x => x.IsLink) != null) return true;
        return targetElement.References.ForwardReferences != null && targetElement.References.ForwardReferences.Any(x => IsLinked(x.TargetNode));
    }

    private static string CreateAddress(string parentKey, string identifier)
    {
        return ElementAddress.Create(parentKey, identifier);
    }
}