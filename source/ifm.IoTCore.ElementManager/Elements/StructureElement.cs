namespace ifm.IoTCore.ElementManager.Elements;

using System.Collections.Generic;
using Common;
using Contracts.Elements;
using Contracts.Elements.Formats;

internal sealed class StructureElement : BaseElement, IStructureElement
{
    public StructureElement(string identifier,
        string address,
        Format format = null, 
        IEnumerable<string> profiles = null, 
        string uid = null,
        bool isHidden = false) : 
        base(Identifiers.Structure, identifier, address, format, profiles, uid, isHidden)
    {
    }
}