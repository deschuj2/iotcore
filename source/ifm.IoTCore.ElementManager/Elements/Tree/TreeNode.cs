namespace ifm.IoTCore.ElementManager.Elements.Tree;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Common;
using Common.Exceptions;
using Contracts.Elements;
using Contracts.Elements.Tree;

internal class TreeNode : ITreeNode
{
    private readonly ReaderWriterLockSlim _lock = new();
    private readonly int _timeout;

    public string Address { get; set; }

    public IBaseElement Parent { get; set; }

    public ReferenceTable References { get; } = new();

    public TreeNode(int timeout)
    {
        _timeout = timeout;
    }

    public void EnterReadLock()
    {
        if (!_lock.TryEnterReadLock(_timeout))
        {
            throw new LockedException($"The element is locked");
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
            throw new LockedException($"The element is locked");
        }
    }

    public void ExitWriteLock()
    {
        _lock.ExitWriteLock();
    }

    public IEnumerable<Reference<IBaseElement>> ForwardReferences
    {
        get
        {
            EnterReadLock();
            try
            {
                return References.ForwardReferences?.Select(item => new Reference<IBaseElement>(item.Identifier, item.SourceNode, item.TargetNode, item.Type, item.Direction)).ToList();
            }
            finally
            {
                ExitReadLock();
            }
        }
    }

    public IEnumerable<Reference<IBaseElement>> InverseReferences
    {
        get
        {
            EnterReadLock();
            try
            {
                return References.InverseReferences?.Select(item => new Reference<IBaseElement>(item.Identifier, item.SourceNode, item.TargetNode, item.Type, item.Direction)).ToList();
            }
            finally
            {
                ExitReadLock();
            }
        }
    }

    public event EventHandler<TreeChangedEventArgs<IBaseElement>> TreeChanged;

    public void OnTreeChanged(object sender, TreeChangedEventArgs<IBaseElement> args)
    {
        TreeChanged.Raise(this, args);
    }

    public void RaiseTreeChanged(TreeChangedEventArgs<IBaseElement> args)
    {
        TreeChanged.Raise(this, args);
    }
}