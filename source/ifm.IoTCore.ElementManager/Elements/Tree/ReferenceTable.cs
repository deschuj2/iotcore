namespace ifm.IoTCore.ElementManager.Elements.Tree;

using System.Collections.Generic;
using System.Linq;
using Contracts.Elements.Tree;

internal class ReferenceTable
{
    public IEnumerable<Reference<BaseElement>> ForwardReferences => _forwardReferences;
    private List<Reference<BaseElement>> _forwardReferences;

    public IEnumerable<Reference<BaseElement>> InverseReferences => _inverseReferences;
    private List<Reference<BaseElement>> _inverseReferences;

    public Reference<BaseElement> AddForwardReference(string identifier, BaseElement sourceNode, BaseElement targetNode, ReferenceTypes type)
    {
        var reference = new Reference<BaseElement>(identifier, sourceNode, targetNode, type, ReferenceDirections.Forward);
        _forwardReferences ??= new List<Reference<BaseElement>>();
        _forwardReferences.Add(reference);
        return reference;
    }

    public Reference<BaseElement> AddInverseReference(string identifier, BaseElement sourceNode, BaseElement targetNode, ReferenceTypes type)
    {
        var reference = new Reference<BaseElement>(identifier, sourceNode, targetNode, type, ReferenceDirections.Inverse);
        _inverseReferences ??= new List<Reference<BaseElement>>();
        _inverseReferences.Add(reference);
        return reference;
    }

    public Reference<BaseElement> RemoveForwardReference(BaseElement sourceNode, BaseElement targetNode)
    {
        var reference = _forwardReferences?.FirstOrDefault(x => x.SourceNode == sourceNode && x.TargetNode == targetNode);
        if (reference == null) return null;
        _forwardReferences.Remove(reference);
        if (_forwardReferences.Count == 0) _forwardReferences = null;
        return reference;
    }

    public Reference<BaseElement> RemoveInverseReference(BaseElement sourceNode, BaseElement targetNode)
    {
        var reference = _inverseReferences?.FirstOrDefault(x => x.SourceNode == sourceNode && x.TargetNode == targetNode);
        if (reference == null) return null;
        _inverseReferences.Remove(reference);
        if (_inverseReferences.Count == 0) _inverseReferences = null;
        return reference;
    }
}
