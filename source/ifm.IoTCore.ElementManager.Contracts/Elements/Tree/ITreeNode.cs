namespace ifm.IoTCore.ElementManager.Contracts.Elements.Tree;

using System;
using System.Collections.Generic;

public interface ITreeNode
{
    /// <summary>
    /// Gets the address of the element. The address uniquely identifies the element.
    /// </summary>
    string Address { get; }

    /// <summary>
    /// Gets the parent element.
    /// </summary>
    IBaseElement Parent { get; }

    /// <summary>
    /// Gets the forward references.
    /// </summary>
    IEnumerable<Reference<IBaseElement>> ForwardReferences { get; }

    /// <summary>
    /// Gets the inverse references.
    /// </summary>
    IEnumerable<Reference<IBaseElement>> InverseReferences { get; }

    /// <summary>
    /// The event that is raised when the tree is modified.
    /// </summary>
    event EventHandler<TreeChangedEventArgs<IBaseElement>> TreeChanged;
}