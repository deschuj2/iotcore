namespace ifm.IoTCore.ElementManager;

using System;
using System.Collections.Concurrent;
using Common;
using Common.Exceptions;
using Contracts;
using Contracts.Elements;
using ifm.IoTCore.ElementManager.Contracts.Elements.Tree;

public class ElementCache : IElementCache
{
    private readonly ConcurrentDictionary<string, IBaseElement> _cache = new(StringComparer.OrdinalIgnoreCase);
    private IBaseElement _root;

    public IBaseElement Root
    {
        get => _root;
        set
        {
            if (_root != null)
            {
                _root.TreeChanged -= RootOnTreeChanged;
            }

            _root = value;

            if (_root != null)
            {
                _root.TreeChanged += RootOnTreeChanged;
            }
        }
    }

    public IBaseElement GetElementByAddress(string address)
    {
        address = ElementAddress.PatchAddress(_root.Identifier, address);

        if (_cache.TryGetValue(address, out var value))
        {
            return value;
        }

        // Address can be address or link address, so we need to split and search the tree
        var tokens = ElementAddress.Split(address);
        if (tokens.Length == 1)
        {
            return tokens[0].Equals(_root.Identifier, StringComparison.OrdinalIgnoreCase) ? _root : null;
        }
        var element = _root;
        for (var idx = 1; idx < tokens.Length; idx++)
        {
            if (element == null) return null;
            element = element.GetElementByIdentifier(tokens[idx]);
        }

        if (element != null) _cache.TryAdd(address, element);
        return element;
    }

    private void RootOnTreeChanged(object sender, TreeChangedEventArgs<IBaseElement> e)
    {
        _cache.Clear();
    }
}
