namespace ifm.IoTCore.ServiceData.Responses;

using System;
using System.Collections;
using System.Collections.Generic;
using Common;

/// <summary>
/// Represents the outgoing data for a IDeviceElement.GetDataMulti service call.
/// </summary>
public class GetDataMultiResponseServiceData : IDictionary<string, CodeDataPair>, IDictionary
{
    private readonly Dictionary<string, CodeDataPair> _dataToSend = new Dictionary<string, CodeDataPair>();

    #region IDictionary<,>

    /// <summary>
    /// Gets or sets the element with the specified key.
    /// </summary>
    /// <param name="key">The key of the element to get or set.</param>
    /// <returns>The element with the specified key.</returns>
    public CodeDataPair this[string key]
    {
        get => _dataToSend[key];
        set => _dataToSend[key] = value;
    }

    /// <summary>
    /// Gets a collection containing the keys of the dictionary.
    /// </summary>
    public ICollection<string> Keys => ((IDictionary<string, CodeDataPair>)_dataToSend).Keys;

    /// <summary>
    /// Gets a collection containing the values of the dictionary.
    /// </summary>
    public ICollection<CodeDataPair> Values => ((IDictionary<string, CodeDataPair>)_dataToSend).Values;

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>The enumerator.</returns>
    public IEnumerator<KeyValuePair<string, CodeDataPair>> GetEnumerator()
    {
        return _dataToSend.GetEnumerator();
    }

    /// <summary>
    /// Adds an element with the provided key and value to the dictionary.
    /// </summary>
    /// <param name="item">The key/value pair.</param>
    public void Add(KeyValuePair<string, CodeDataPair> item)
    {
        if (!ContainsKey(item.Key))
        {
            _dataToSend.Add(item.Key, item.Value);
        }
    }

    /// <summary>
    /// Adds an element with the provided key and value to the dictionary.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    public void Add(string key, CodeDataPair value)
    {
        if (!ContainsKey(key))
        {
            _dataToSend.Add(key, value);
        }
    }

    /// <summary>
    /// Determines whether the dictionary contains an element with the specified key.
    /// </summary>
    /// <param name="key">The key to locate in the dictionary.</param>
    /// <returns>true if the dictionary contains an element with the key; otherwise false.</returns>
    public bool ContainsKey(string key)
    {
        return _dataToSend.ContainsKey(key);
    }

    /// <summary>
    /// Removes the element with the specified key from the dictionary.
    /// </summary>
    /// <param name="key">The key of the element to remove.</param>
    /// <returns>true if the element is successfully removed; otherwise false.</returns>
    public bool Remove(string key)
    {
        return _dataToSend.Remove(key);
    }

    /// <summary>
    /// Gets the value associated with the specified key.
    /// </summary>
    /// <param name="key">The key whose value to get.</param>
    /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise null.</param>
    /// <returns>true if the dictionary contains an element with the specified key; otherwise false.</returns>
    public bool TryGetValue(string key, out CodeDataPair value)
    {
        return _dataToSend.TryGetValue(key, out value);
    }

    /// <summary>
    /// Determines whether the dictionary contains an element with the specified key.
    /// </summary>
    /// <param name="item">The key/value pair.</param>
    /// <returns>true if the dictionary contains an element with the key; otherwise false.</returns>
    public bool Contains(KeyValuePair<string, CodeDataPair> item)
    {
        return _dataToSend.ContainsKey(item.Key);
    }

    /// <summary>
    /// Removes the element with the specified key from the dictionary.
    /// </summary>
    /// <param name="item">The key/value pair.</param>
    /// <returns>true if the element is successfully removed; otherwise false.</returns>
    public bool Remove(KeyValuePair<string, CodeDataPair> item)
    {
        return _dataToSend.Remove(item.Key);
    }

    /// <summary>
    /// Copies the elements of the dictionary to an Array, starting at a particular Array index.
    /// </summary>
    /// <param name="array">The destination array.</param>
    /// <param name="arrayIndex">The start index at which copying begins.</param>
    public void CopyTo(KeyValuePair<string, CodeDataPair>[] array, int arrayIndex)
    {
        ((IDictionary<string, CodeDataPair>)_dataToSend).CopyTo(array, arrayIndex);
    }

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>The enumerator.</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_dataToSend).GetEnumerator();
    }

    /// <summary>
    /// Removes all items from the dictionary.
    /// </summary>
    public void Clear()
    {
        _dataToSend.Clear();
    }

    /// <summary>
    /// Gets the number of elements contained in the dictionary.
    /// </summary>
    public int Count => _dataToSend.Count;


    #endregion

    #region IDictionary

    /// <summary>
    /// Gets a value indicating whether the dictionary object has a fixed size.
    /// </summary>
    public bool IsFixedSize => ((IDictionary)_dataToSend).IsFixedSize;

    /// <summary>
    /// Gets a value indicating whether the dictionary object is read-only.
    /// </summary>
    public bool IsReadOnly => ((IDictionary)_dataToSend).IsReadOnly;

    /// <summary>
    /// Gets or sets the element with the specified key.
    /// </summary>
    /// <param name="key">The key of the element to get or set.</param>
    /// <returns>The element with the specified key, or null if the key does not exist.</returns>
    public object this[object key]
    {
        get => ((IDictionary)_dataToSend)[key];
        set => ((IDictionary)_dataToSend)[key] = value;
    }

    /// <summary>
    /// Gets a collection containing the keys of the dictionary.
    /// </summary>
    ICollection IDictionary.Keys => ((IDictionary)_dataToSend).Keys;

    /// <summary>
    /// Gets a collection containing the values of the dictionary.
    /// </summary>
    ICollection IDictionary.Values => ((IDictionary)_dataToSend).Values;

    /// <summary>
    /// Adds an element with the provided key and value to the dictionary.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    public void Add(object key, object value)
    {
        ((IDictionary)_dataToSend).Add(key, value);
    }

    /// <summary>
    /// Determines whether the dictionary contains an element with the specified key.
    /// </summary>
    /// <param name="key">The key/value pair.</param>
    /// <returns>true if the dictionary contains an element with the key; otherwise false.</returns>
    public bool Contains(object key)
    {
        return ((IDictionary)_dataToSend).Contains(key);
    }

    /// <summary>
    /// Copies the elements of the dictionary to an Array, starting at a particular Array index.
    /// </summary>
    /// <param name="array">The destination array.</param>
    /// <param name="index">The start index at which copying begins.</param>
    public void CopyTo(Array array, int index)
    {
        ((IDictionary)_dataToSend).CopyTo(array, index);
    }

    /// <summary>
    /// Gets an object that can be used to synchronize access to the dictionary.
    /// </summary>
    public object SyncRoot => ((IDictionary)_dataToSend).SyncRoot;

    /// <summary>
    /// Gets a value indicating whether access to the dictionary is synchronized (thread safe).
    /// </summary>
    public bool IsSynchronized => ((IDictionary)_dataToSend).IsSynchronized;

    /// <summary>
    /// Removes the element with the specified key from the dictionary.
    /// </summary>
    /// <param name="key">The key of the element to remove.</param>
    public void Remove(object key)
    {
        ((IDictionary)_dataToSend).Remove(key);
    }

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>The enumerator.</returns>
    IDictionaryEnumerator IDictionary.GetEnumerator()
    {
        return ((IDictionary)_dataToSend).GetEnumerator();
    }

    #endregion

}