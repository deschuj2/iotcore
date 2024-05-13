namespace ifm.IoTCore.ElementManager.Contracts.Elements.Tree;

/// <summary>
/// Defines the actions that caused a tree changed event. 
/// </summary>
public enum TreeChangedActions
{
    /// <summary>
    /// An child was added.
    /// </summary>
    ChildAdded,

    /// <summary>
    /// An child was removed.
    /// </summary>
    ChildRemoved,

    /// <summary>
    /// A link was added.
    /// </summary>
    LinkAdded,

    /// <summary>
    /// A link was removed.
    /// </summary>
    LinkRemoved,

    /// <summary>
    /// Multiple changes done.
    /// </summary>
    TreeChanged
}

