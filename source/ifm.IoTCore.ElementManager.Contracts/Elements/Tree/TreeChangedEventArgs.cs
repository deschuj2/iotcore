namespace ifm.IoTCore.ElementManager.Contracts.Elements.Tree;

using System;

/// <summary>
/// Provides data for the tree changed event.
/// </summary>
public class TreeChangedEventArgs<T> : EventArgs
{
    /// <summary>
    /// Gets the action that caused the event.
    /// </summary>
    public TreeChangedActions Action { get; }

    /// <summary>
    /// Gets the node that was affected by the change.
    /// </summary>
    public T SourceNode { get; }

    /// <summary>
    /// Gets the node involved in the change.
    /// </summary>
    public T TargetNode { get; }

    /// <summary>
    /// Gets the identifier for a link, if it is a link event.
    /// </summary>
    public string Identifier { get; }

    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="action">The action that caused the event.</param>
    /// <param name="sourceNode">The node that was affected by the change.</param>
    /// <param name="targetNode">The node involved in the change.</param>
    /// <param name="identifier">The identifier for a link, if it is a link event.</param>
    public TreeChangedEventArgs(TreeChangedActions action, T sourceNode, T targetNode, string identifier = null)
    {
        Action = action;
        SourceNode = sourceNode;
        TargetNode = targetNode;
        Identifier = identifier;
    }
}