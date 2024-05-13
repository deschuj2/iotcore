namespace ifm.IoTCore.ElementManager.Contracts.Elements.Tree;

/// <summary>
/// Defines a reference between two nodes.
/// </summary>
public class Reference<T>
{
    /// <summary>
    /// The identifier of the reference.
    /// </summary>
    public string Identifier { get; }

    /// <summary>
    /// The source node of the reference.
    /// </summary>
    public T SourceNode { get; }

    /// <summary>
    /// The target node of the reference.
    /// </summary>
    public T TargetNode { get; }

    /// <summary>
    /// The type of the reference.
    /// </summary>
    public ReferenceTypes Type { get; }

    /// <summary>
    /// The direction of the reference.
    /// </summary>
    public ReferenceDirections Direction { get; }

    /// <summary>
    /// Initializes a new instance of the TreeNodeReference class.
    /// </summary>
    /// <param name="identifier">The identifier of the reference.</param>
    /// <param name="sourceNode">The source node of the reference.</param>
    /// <param name="targetNode">The target node of the reference.</param>
    /// <param name="type">The type of the reference.</param>
    /// <param name="direction">The direction of the reference.</param>
    public Reference(string identifier, T sourceNode, T targetNode, ReferenceTypes type, ReferenceDirections direction)
    {
        Identifier = identifier;
        SourceNode = sourceNode;
        TargetNode = targetNode;
        Type = type;
        Direction = direction;
    }

    /// <summary>
    /// Checks if the reference is a child reference.
    /// </summary>
    public bool IsChild => Type == ReferenceTypes.Child;

    /// <summary>
    /// Checks if the reference is a link reference.
    /// </summary>
    public bool IsLink => Type == ReferenceTypes.Link;

    /// <summary>
    /// Checks if the reference is a forward reference.
    /// </summary>
    public bool IsForward => Direction == ReferenceDirections.Forward;

    /// <summary>
    /// Checks if the reference is an inverse reference.
    /// </summary>
    public bool IsInverse => Direction == ReferenceDirections.Inverse;

    /// <summary>
    /// Converts this instance to a human readable string.
    /// </summary>
    public override string ToString()
    {
        return IsForward ? $"{SourceNode} --> {TargetNode}" : $"{SourceNode} <-- {TargetNode}";
    }
}