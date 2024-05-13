namespace ifm.IoTCore.Common;

using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Provides extension methods for collection types.
/// </summary>
public static class CollectionExtensions
{
    /// <summary>
    /// Removes all items from a dictionary which match the specified predicate.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    /// <param name="dic">The dictionary.</param>
    /// <param name="predicate">The function to test each element for a condition.</param>
    public static void RemoveAll<TKey, TValue>(this IDictionary<TKey, TValue> dic, Predicate<TValue> predicate)
    {
        if (dic == null) throw new ArgumentNullException(nameof(dic));
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));

        var keys = dic.Keys.Where(x => predicate(dic[x])).ToList();
        foreach (var key in keys)
        {
            dic.Remove(key);
        }
    }

    /// <summary>
    /// Adds an item to a list only if the item is not null.
    /// </summary>
    /// <typeparam name="T">Type of the items in the list.</typeparam>
    /// <param name="list">The list.</param>
    /// <param name="item">The item to add to the end of the list.</param>
    public static void AddIfNotNull<T>(this IList<T> list, T item)
    {
        if (list == null) throw new ArgumentNullException(nameof(list));

        if (item != null)
        {
            list.Add(item);
        }
    }

    public static bool HasDuplicates(this IList<string> list)
    {
        return list.Count != list.Distinct().Count();
    }
}