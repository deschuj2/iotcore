namespace ifm.IoTCore.ElementManager.Contracts.Elements;

using System;
using System.Collections.Generic;
using Formats;
using Tree;

/// <summary>
/// Provides functionality to interact with a base element.
/// </summary>
public interface IBaseElement : ITreeNode
{
    /// <summary>
    /// Gets the type of the element.
    /// </summary>
    string Type { get; }

    /// <summary>
    /// Gets the identifier of the element.
    /// </summary>
    string Identifier { get; }

    /// <summary>
    /// Gets the format of the element.
    /// </summary>
    Format Format { get; }

    /// <summary>
    /// Gets the profiles of the element.
    /// </summary>
    IEnumerable<string> Profiles { get; }

    /// <summary>
    /// Gets all referenced elements.
    /// </summary>
    IEnumerable<IBaseElement> Subs { get; }

    /// <summary>
    /// Gets the tags of the element.
    /// </summary>
    IEnumerable<string> Tags { get; }

    /// <summary>
    /// Gets the application or profile specific information about the element.
    /// </summary>
    public IDictionary<string, object> Infos { get; }

    /// <summary>
    /// Gets the unique identifier of the element.
    /// </summary>
    string UId { get; }

    /// <summary>
    /// Gets or sets the hidden status of the element.
    /// </summary>
    bool IsHidden { get; set; }

    /// <summary>
    /// Adds a profile.
    /// </summary>
    /// <param name="profile">The name of the profile to add.</param>
    void AddProfile(string profile);

    /// <summary>
    /// Removes a profile.
    /// </summary>
    /// <param name="profile">The name of the profile to remove.</param>
    void RemoveProfile(string profile);

    /// <summary>
    /// Checks if this element has the profile.
    /// </summary>
    /// <param name="profile">The name of the profile.</param>
    /// <returns>true, if this element has the profile, otherwise false.</returns>
    bool HasProfile(string profile);

    /// <summary>
    /// Adds a tag.
    /// </summary>
    /// <param name="tag">The name of the tag to add.</param>
    void AddTag(string tag);

    /// <summary>
    /// Removes a tag.
    /// </summary>
    /// <param name="tag">The name of the tag to remove.</param>
    void RemoveTag(string tag);

    /// <summary>
    /// Checks if this element has the tag.
    /// </summary>
    /// <param name="tag">The name of the tag.</param>
    /// <returns>true, if this element has the tag, otherwise false.</returns>
    bool HasTag(string tag);

    /// <summary>
    /// Adds the element info with the associated key.
    /// </summary>
    /// <typeparam name="T">The type of the element info.</typeparam>
    /// <param name="key">The key of the element info.</param>
    /// <param name="value">The element info.</param>
    void AddInfo<T>(string key, T value);

    /// <summary>
    /// Removes the element info with the associated key.
    /// </summary>
    /// <param name="key">The key of the element info.</param>
    void RemoveInfo(string key);

    /// <summary>
    /// Gets the element info which is associated with the specified key.
    /// </summary>
    /// <typeparam name="T">The type of the element info.</typeparam>
    /// <param name="key">The key of the element info to get.</param>
    /// <returns>The user data.</returns>
    T GetInfo<T>(string key);

    /// <summary>
    /// Adds the user data with the associated key.
    /// </summary>
    /// <typeparam name="T">The type of the user data.</typeparam>
    /// <param name="key">The key of the user data.</param>
    /// <param name="value">The user data.</param>
    void AddUserData<T>(string key, T value);

    /// <summary>
    /// Removes the user data with the associated key.
    /// </summary>
    /// <param name="key">The key of the user data.</param>
    void RemoveUserData(string key);

    /// <summary>
    /// Gets the user data which is associated with the specified key.
    /// </summary>
    /// <typeparam name="T">The type of the user data.</typeparam>
    /// <param name="key">The key of the user data to get.</param>
    /// <returns>The user data.</returns>
    T GetUserData<T>(string key);

    /// <summary>
    /// Gets the element with the specified identifier.
    /// </summary>
    /// <param name="identifier">The identifier of the element.</param>
    /// <returns>The requested element if it exists; otherwise null.</returns>
    IBaseElement GetElementByIdentifier(string identifier);

    /// <summary>
    /// Gets the first element with the specified predicate.
    /// </summary>
    /// <param name="predicate">The predicate to match.</param>
    /// <param name="recurse">If true the tree is search is recursively searched; otherwise not</param>
    /// <returns>The requested element if it exists; otherwise null.</returns>
    IBaseElement GetElementByPredicate(Predicate<IBaseElement> predicate, bool recurse = true);

    /// <summary>
    /// Gets all elements with the specified predicate.
    /// </summary>
    /// <param name="predicate">The predicate to match.</param>
    /// <param name="recurse">If true the tree is search is recursively searched; otherwise not</param>
    /// <returns>The elements that match the predicate if successful; otherwise null.</returns>
    IEnumerable<IBaseElement> GetElementsByPredicate(Predicate<IBaseElement> predicate, bool recurse = true);
}