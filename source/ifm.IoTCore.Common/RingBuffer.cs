namespace ifm.IoTCore.Common;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;

/// <summary>
/// Represents a ring buffer.
/// </summary>
/// <typeparam name="T">The type of the items in the buffer.</typeparam>
public class RingBuffer<T> : IEnumerable<T>
{
    private readonly T[] _buffer;
    private int _pos;
    private int _size;

    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="capacity">The capacity of the buffer.</param>
    public RingBuffer(int capacity)
    {
        if (capacity <= 0) throw new ArgumentOutOfRangeException(nameof(capacity));

        _buffer = new T[capacity];
    }

    /// <summary>
    /// Adds a new item to the buffer.
    /// </summary>
    /// <param name="item">The item to add.</param>
    public void Add(T item)
    {
        _buffer[_pos] = item;
        _pos = (_pos + 1) % _buffer.Length;
        if (_size < _buffer.Length) _size++;
    }

    /// <summary>
    /// Removes all items from the buffer.
    /// </summary>
    public void Clear()
    {
        Array.Clear(_buffer, 0, _buffer.Length);
        _pos = 0;
        _size = 0;
    }

    /// <summary>
    /// Gets the first item from the buffer.
    /// </summary>
    /// <returns></returns>
    public T GetFirst()
    {
        return _size < _buffer.Length ? _buffer[0] : _buffer[_pos];
    }

    /// <summary>
    /// Gets the last item from the buffer.
    /// </summary>
    /// <returns></returns>
    public T GetLast()
    {
        return _buffer[(_pos + _buffer.Length - 1) % _buffer.Length];
    }

    /// <summary>
    /// Indicates whether the buffer is empty.
    /// </summary>
    public bool IsEmpty => _size == 0;

    /// <summary>
    /// Gets the number of items in the buffer.
    /// </summary>
    public int Count => _size;

    /// <summary>
    /// Gets the capacity of the buffer.
    /// </summary>
    public int Capacity => _buffer.Length;

    /// <summary>
    /// Copies the items of the buffer to a new array.
    /// </summary>
    /// <returns>An array containing copies of the items of the buffer.</returns>
    public T[] ToArray()
    {
        var buf = new T[_size];
        for (var i = 0; i < _size; i++)
        {
            buf[i] = InternalGetAt(i);
        }
        return buf;
    }

    /// <summary>
    /// Returns an enumerator that iterates through the buffer.
    /// </summary>
    /// <returns>An enumerator that can be used to iterate through the buffer.</returns>
    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    /// Returns an enumerator that iterates through the buffer.
    /// </summary>
    /// <returns>An enumerator that can be used to iterate through the buffer.</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private IEnumerator<T> GetEnumerator()
    {
        for (var i = 0; i < _size; i++)
        {
            yield return InternalGetAt(i);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private T InternalGetAt(int pos)
    {
        //return _buffer[(_pos + pos) % _buffer.Length];
        return _size < _buffer.Length ? _buffer[pos] : _buffer[(_pos + pos) % _buffer.Length];
    }
}

/// <summary>
/// Represents an observable ring buffer.
/// </summary>
/// <typeparam name="T">The type of the items in the buffer.</typeparam>
public class ObservableRingBuffer<T> : RingBuffer<T>, INotifyCollectionChanged
{
    /// <summary>
    /// The CollectionChanged event handler.
    /// </summary>
    public event NotifyCollectionChangedEventHandler CollectionChanged;

    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="capacity">The capacity of the buffer</param>
    public ObservableRingBuffer(int capacity) : base(capacity)
    {
    }

    /// <summary>
    /// Adds a new item to the buffer and raises a collection changed event.
    /// </summary>
    /// <param name="item">The item to add</param>
    public new void Add(T item)
    {
        base.Add(item);

        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
    }

    /// <summary>
    /// Removes all items from the buffer and raises a collection changed event.
    /// </summary>
    public new void Clear()
    {
        base.Clear();

        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }
}