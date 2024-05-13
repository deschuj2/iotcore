namespace ifm.IoTCore.ElementManager.Elements;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Contracts.Elements;
using Contracts.Elements.Formats;
using Tree;

internal class BaseElement : TreeNode, IBaseElement
{
    private ConcurrentDictionary<string, object> _profiles;
    private ConcurrentDictionary<string, object> _tags;
    private ConcurrentDictionary<string, object> _infos;
    private ConcurrentDictionary<string, object> _userData;

    public string Type { get; }
    public string Identifier { get; }
    public Format Format { get; protected set; }
    public IEnumerable<string> Profiles => _profiles?.Keys.ToArray();
    public IEnumerable<string> Tags => _tags?.Keys.ToArray();
    public IDictionary<string, object> Infos => _infos;
    public string UId { get; }
    public bool IsHidden { get; set; }

    private static Regex IdentifierValidator => new("^[a-zA-Z0-9_\\-\\]\\[]*$");
    private static Regex ProfileValidator => new("^[a-zA-Z0-9_\\-/\\]\\[/]*$");

    public BaseElement(string type,
        string identifier,
        string address,
        Format format = null,
        IEnumerable<string> profiles = null,
        string uid = null,
        bool isHidden = false,
        int timeout = 5000) : base(timeout)
    {
        if (string.IsNullOrEmpty(type)) throw new ArgumentNullException(nameof(type));
        if (string.IsNullOrEmpty(identifier)) throw new ArgumentNullException(nameof(identifier));
        if (!IdentifierValidator.IsMatch(identifier)) throw new ArgumentException(identifier);

        Type = type;
        Identifier = identifier;
        Address = address;
        Format = format;
        if (profiles != null)
        {
            _profiles = new ConcurrentDictionary<string, object>();
            foreach (var profile in profiles)
            {
                _profiles.TryAdd(profile, null);
            }
        }
        UId = uid;
        IsHidden = isHidden;
    }

    public void AddProfile(string profile)
    {
        if (!ProfileValidator.IsMatch(profile)) throw new ArgumentException(profile);

        _profiles ??= new ConcurrentDictionary<string, object>();
        _profiles.TryAdd(profile, null);
    }

    public void RemoveProfile(string profile)
    {
        if (_profiles == null) return;
        if (!_profiles.TryRemove(profile, out _)) return;
        if (_profiles.IsEmpty) _profiles = null;
    }

    public bool HasProfile(string profile)
    {
        return _profiles != null && _profiles.ContainsKey(profile);
    }

    public void AddTag(string tag)
    {
        _tags ??= new ConcurrentDictionary<string, object>();
        _tags.TryAdd(tag, null);
    }

    public void RemoveTag(string tag)
    {
        if (_tags == null) return;
        if (!_tags.TryRemove(tag, out _)) return;
        if (_tags.IsEmpty) _tags = null;
    }

    public bool HasTag(string tag)
    {
        return _tags != null && _tags.ContainsKey(tag);
    }

    public void AddInfo<T>(string key, T value)
    {
        _infos ??= new ConcurrentDictionary<string, object>();
        _infos.TryAdd(key, value);
    }

    public void RemoveInfo(string key)
    {
        if (_infos == null) return;
        _infos.TryRemove(key, out _);
        if (_infos.IsEmpty) _infos = null;
    }

    public T GetInfo<T>(string key)
    {
        if (_infos.TryGetValue(key, out var value))
        {
            return (T)value;
        }
        return default;
    }

    public void AddUserData<T>(string key, T value)
    {
        _userData ??= new ConcurrentDictionary<string, object>();
        _userData.TryAdd(key, value);
    }

    public void RemoveUserData(string key)
    {
        if (_userData == null) return;
        _userData.TryRemove(key, out _);
        if (_userData.IsEmpty) _userData = null;
    }

    public T GetUserData<T>(string key)
    {
        if (_userData.TryGetValue(key, out var value))
        {
            return (T)value;
        }
        return default;
    }

    public IEnumerable<IBaseElement> Subs
    {
        get
        {
            EnterReadLock();
            try
            {
                return References.ForwardReferences?.Select(x => x.TargetNode).ToList();
            }
            finally
            {
                ExitReadLock();
            }
        }
    }

    public IBaseElement GetElementByIdentifier(string identifier)
    {
        var forwardReferences = ForwardReferences;
        if (forwardReferences == null) return null;
        foreach (var item in forwardReferences)
        {
            if (item.Identifier.Equals(identifier, StringComparison.OrdinalIgnoreCase)) return item.TargetNode;
        }
        return null;
    }

    public IBaseElement GetElementByPredicate(Predicate<IBaseElement> predicate, bool recurse = true)
    {
        return GetElementByPredicate(this, predicate, recurse);
    }

    public IEnumerable<IBaseElement> GetElementsByPredicate(Predicate<IBaseElement> predicate, bool recurse = true)
    {
        var result = new List<IBaseElement>();
        GetElementsByPredicate(this, predicate, recurse, result);
        return result;
    }

    // js2023/10/18: If moved to element manager use References.ForwardReferences because inside element manager the tree is locked
    private static IBaseElement GetElementByPredicate(IBaseElement element, Predicate<IBaseElement> predicate, bool recurse)
    {
        if (predicate(element)) return element;
        if (!recurse) return null;
        var forwardReferences = element.ForwardReferences;
        if (forwardReferences == null) return null;
        foreach (var item in forwardReferences)
        {
            var result = GetElementByPredicate(item.TargetNode, predicate, true);
            if (result != null) return result;
        }
        return null;
    }

    // js2023/10/18: If moved to element manager use References.ForwardReferences because inside element manager the tree is locked
    private static void GetElementsByPredicate(IBaseElement element, Predicate<IBaseElement> predicate, bool recurse, ICollection<IBaseElement> result)
    {
        if (predicate(element)) result.Add(element);
        if (!recurse) return;
        var forwardReferences = element.ForwardReferences;
        if (forwardReferences == null) return;
        foreach (var item in forwardReferences)
        {
            if (item.IsChild) GetElementsByPredicate(item.TargetNode, predicate, true, result);
        }
    }

    public override string ToString()
    {
        return Identifier;
    }
}