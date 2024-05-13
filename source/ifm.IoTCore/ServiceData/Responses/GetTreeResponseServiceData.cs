namespace ifm.IoTCore.ServiceData.Responses;

using System.Collections.Generic;
using System.Linq;
using ElementManager.Contracts.Elements;
using Common;
using Common.Variant;
using ElementManager.Contracts.Elements.Formats;

/// <summary>
/// Represents the outgoing data for a DeviceElement.GetTree service call.
/// </summary>
public class GetTreeResponseServiceData
{
    /// <summary>
    /// Represents a tree element for a DeviceElement.GetTree service call.
    /// </summary>
    public class TreeElement
    {
        /// <summary>
        /// The type of the element.
        /// </summary>
        [VariantProperty("type", Required = true)]
        public string Type { get; set; }

        /// <summary>
        /// The identifier of the element.
        /// </summary>
        [VariantProperty("identifier", Required = true)]
        public string Identifier { get; set; }

        /// <summary>
        /// The address of the element.
        /// </summary>
        [VariantProperty("adr", IgnoredIfNull = true)]
        public string Address { get; set; }

        /// <summary>
        /// If the element is a linked element the original address of elements.
        /// </summary>
        [VariantProperty("link", IgnoredIfNull = true)]
        public string Link { get; set; }

        /// <summary>
        /// The format of the element.
        /// </summary>
        [VariantProperty("format", IgnoredIfNull = true)]
        public Format Format { get; set; }

        /// <summary>
        /// The list of profiles the element belongs to.
        /// </summary>
        [VariantProperty("profiles", IgnoredIfNull = true)]
        public List<string> Profiles { get; set; }

        /// <summary>
        /// The list of tags the element belongs to.
        /// </summary>
        [VariantProperty("tags", IgnoredIfNull = true)]
        public List<string> Tags { get; set; }

        /// <summary>
        /// The element infos.
        /// </summary>
        [VariantProperty("infos", IgnoredIfNull = true)]
        public Dictionary<string, object> Infos { get; set; }

        /// <summary>
        /// The unique identifier of the element.
        /// </summary>
        [VariantProperty("uid", IgnoredIfNull = true)]
        public string UId { get; set; }

        /// <summary>
        /// The value, if the element is of type data and has the profile "const_value".
        /// </summary>
        [VariantProperty("value", IgnoredIfNull = true)]
        public Variant Value { get; set; }

        /// <summary>
        /// The list of child elements of the element.
        /// </summary>
        [VariantProperty("subs", IgnoredIfNull = true)]
        public List<TreeElement> Subs { get; set; }

        /// <summary>
        /// The parameterless constructor for the variant converter.
        /// </summary>
        [VariantConstructor]
        public TreeElement()
        {
        }
    }

    /// <summary>
    /// The type of the root element in the tree.
    /// </summary>
    [VariantProperty("type", Required = true)]
    public string Type
    {
        get => _element.Type;
        set => _element.Type = value;
    }

    /// <summary>
    /// The identifier of the root element in the tree.
    /// </summary>
    [VariantProperty("identifier", Required = true)]
    public string Identifier
    {
        get => _element.Identifier;
        set => _element.Identifier = value;
    }

    /// <summary>
    /// The address of the root element in the tree.
    /// </summary>
    [VariantProperty("adr", IgnoredIfNull = true)]
    public string Address
    {
        get => _element.Address;
        set => _element.Address = value;
    }

    /// <summary>
    /// The format of the element.
    /// </summary>
    [VariantProperty("format", IgnoredIfNull = true)]
    public Format Format
    {
        get => _element.Format;
        set => _element.Format = value;
    }

    /// <summary>
    /// The list of profiles of the root element.
    /// </summary>
    [VariantProperty("profiles", IgnoredIfNull = true)]
    public List<string> Profiles
    {
        get => _element.Profiles;
        set => _element.Profiles = value;
    }

    /// <summary>
    /// The list of tags of the root element.
    /// </summary>
    [VariantProperty("profiles", IgnoredIfNull = true)]
    public List<string> Tags
    {
        get => _element.Tags;
        set => _element.Tags = value;
    }

    /// <summary>
    /// The infos of the element.
    /// </summary>
    [VariantProperty("infos", IgnoredIfNull = true)]
    public Dictionary<string, object> Infos
    {
        get => _element.Infos;
        set => _element.Infos = value;
    }

    /// <summary>
    /// The unique identifier of the element.
    /// </summary>
    [VariantProperty("uid", IgnoredIfNull = true)]
    public string UId
    {
        get => _element.UId;
        set => _element.UId = value;
    }

    /// <summary>
    /// The value, if the element is of type data and has the profile "const_value".
    /// </summary>
    [VariantProperty("value", IgnoredIfNull = true)]
    public Variant Value
    {
        get => _element.Value;
        set => _element.Value = value;
    }

    /// <summary>
    /// The list of child elements in the tree.
    /// </summary>
    [VariantProperty("subs", IgnoredIfNull = true)]
    public List<TreeElement> Subs
    {
        get => _element.Subs;
        set => _element.Subs = value;
    }

    /// <summary>
    /// The root element in the tree.
    /// </summary>
    [VariantProperty("element", Ignored = true)]
    public TreeElement Element => _element;

    private readonly TreeElement _element;

    /// <summary>
    /// The parameterless constructor for the variant converter.
    /// </summary>
    [VariantConstructor]
    public GetTreeResponseServiceData()
    {
        _element = new TreeElement();
    }

    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="element">The top element for the tree response.</param>
    /// <param name="level">The number of levels to get from the tree. </param>
    /// <param name="expandConstValues">If true, the data elements with profile "const_value" should show their values in "value" property.</param>
    /// <param name="expandLinks">The value to use if expand const values is set to true.</param>
    public GetTreeResponseServiceData(IBaseElement element, int level, bool expandConstValues, bool expandLinks)
    {
        _element = CreateElement(element.Address, element.Identifier, element, false, 0, level, expandConstValues, expandLinks);
    }

    private static TreeElement CreateElement(string address, string identifier, IBaseElement element, bool isLink, int currentLevel, int level, bool expandConstValues, bool expandLinks)
    {
        var treeElement = new TreeElement();

        if (isLink)
        {
            if (expandLinks)
            {
                treeElement.Type = element.Type;
                treeElement.Identifier = identifier;
                treeElement.Address = address;
                treeElement.Link = element.Address;
                treeElement.Format = element.Format;
                treeElement.Profiles = element.Profiles?.ToList();
                treeElement.Tags = element.Tags?.ToList();
                treeElement.Infos = element.Infos != null ? new Dictionary<string, object>(element.Infos) : null;
                treeElement.UId = element.UId;
                treeElement.Value = expandConstValues ? ExpandConstValue(element) : null;
            }
            else
            {
                treeElement.Type = "unknown";
                treeElement.Identifier = identifier;
                treeElement.Address = address;
                treeElement.Link = element.Address;
            }
        }
        else
        {
            treeElement.Type = element.Type;
            treeElement.Identifier = identifier;
            treeElement.Address = address;
            treeElement.Format = element.Format;
            treeElement.Profiles = element.Profiles?.ToList();
            treeElement.Tags = element.Tags?.ToList();
            treeElement.Infos = element.Infos != null ? new Dictionary<string, object>(element.Infos) : null;
            treeElement.UId = element.UId;
            treeElement.Value = expandConstValues ? ExpandConstValue(element) : null;
        }

        if (!isLink || expandLinks)
        {
            if (currentLevel < level && element.ForwardReferences != null)
            {
                treeElement.Subs = new List<TreeElement>();
                foreach (var item in element.ForwardReferences)
                {
                    if (item.TargetNode.IsHidden) continue;
                    treeElement.Subs.Add(CreateElement(ElementAddress.Create(treeElement.Address, item.Identifier), item.Identifier, item.TargetNode, isLink || item.IsLink, currentLevel + 1, level, expandConstValues, expandLinks));
                }
            }
        }
        return treeElement;
    }

    private static Variant ExpandConstValue(IBaseElement element)
    {
        if (element.HasProfile("const_value") && element is IReadDataElement dataElement)
        {
            try
            {
                return dataElement.GetValue();
            }
            catch
            {
                // Ignore
                // ToDo: Use logger
            }
        }
        return null;
    }
}