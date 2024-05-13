namespace ifm.IoTCore.Common.UnitTests;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using NUnit.Framework;

[TestFixture]
public class RingBufferTests
{
    [Test]
    public void CreateRingBuffer_InvalidParameter_Throws()
    {
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ArgumentOutOfRangeException>(() => new RingBuffer<int>(-1));
    }

    [Test]
    public void CreateRingBuffer_Success()
    {
        var buffer = new RingBuffer<int>(100);

        Assert.That(buffer.IsEmpty);
        Assert.That(buffer.Count == 0);
        Assert.That(buffer.Capacity == 100);
    }

    [Test]
    public void AddItem_Success()
    {
        var buffer = new RingBuffer<int>(1) { 10 };

        Assert.That(buffer.IsEmpty,Is.False);
        Assert.That(buffer.Count == 1);

        var first = buffer.GetFirst();
        Assert.That(first == 10);

        var last = buffer.GetLast();
        Assert.That(last == 10);

        buffer.Clear();

        Assert.That(buffer.IsEmpty);
        Assert.That(buffer.Count == 0);
    }

    [Test]
    public void AddItems1_Success()
    {
        var buffer = new RingBuffer<int>(1) { 10, 20, 30, 40, 50 };

        var first = buffer.GetFirst();
        Assert.That(first == 50);

        var last = buffer.GetLast();
        Assert.That(last == 50);
    }

    [Test]
    public void AddItems2_Success()
    {
        var buffer = new RingBuffer<int>(3) { 10, 20, 30, 40, 50 };

        var first = buffer.GetFirst();
        Assert.That(first == 30);

        var last = buffer.GetLast();
        Assert.That(last == 50);
    }

    [Test]
    public void AddItems3_Success()
    {
        var buffer = new RingBuffer<int>(5) { 10, 20, 30, 40, 50 };

        var first = buffer.GetFirst();
        Assert.That(first == 10);

        var last = buffer.GetLast();
        Assert.That(last == 50);
    }

    [Test]
    public void AddItems4_Success()
    {
        var buffer = new RingBuffer<int>(10) { 10, 20, 30, 40, 50 };

        var first = buffer.GetFirst();
        Assert.That(first == 10);

        var last = buffer.GetLast();
        Assert.That(last == 50);
    }

    [Test]
    public void AddItemsInLoop_Success()
    {
        var buffer = new RingBuffer<int>(100);

        var i = 0;
        for (;i < 50; i++)
        {
            buffer.Add(i);
        }

        var first = buffer.GetFirst();
        Assert.That(first == 0);

        var last = buffer.GetLast();
        Assert.That(last == 49);

        i = 0;
        foreach (var item in buffer)
        {
            Assert.That(item == i++);
        }

        for (i = 50; i < 150; i++)
        {
            buffer.Add(i);
        }

        first = buffer.GetFirst();
        Assert.That(first == 50);

        last = buffer.GetLast();
        Assert.That(last == 149);

        i = 50;
        foreach (var item in buffer)
        {
            Assert.That(item == i++);
        }
    }

    [Test]
    public void ToArray_Success()
    {
        var buffer = new RingBuffer<int>(10) { 10, 20, 30, 40, 50 };

        var array = buffer.ToArray();
        Assert.That(array.SequenceEqual(buffer));
    }

    [Test]
    public void Enumerate_Success()
    {
        var buffer = new RingBuffer<int>(10) { 10, 20, 30, 40, 50 };

        var e1 = ((IEnumerable<int>)buffer).GetEnumerator();
        e1.MoveNext();
        Assert.That(10 == e1.Current);
        e1.MoveNext();
        Assert.That(20 == e1.Current);
        e1.MoveNext();
        Assert.That(30 == e1.Current);
        e1.MoveNext();
        Assert.That(40 == e1.Current);
        e1.MoveNext();
        Assert.That(50 == e1.Current);

        e1.Dispose();

        var e2 = ((IEnumerable)buffer).GetEnumerator();
        e2.MoveNext();
        Assert.That(e2.Current != null && 10 == (int)e2.Current);
        e2.MoveNext();
        Assert.That(e2.Current != null && 20 == (int)e2.Current);
        e2.MoveNext();
        Assert.That(e2.Current != null && 30 == (int)e2.Current);
        e2.MoveNext();
        Assert.That(e2.Current != null && 40 == (int)e2.Current);
        e2.MoveNext();
        Assert.That(e2.Current != null && 50 == (int)e2.Current);
    }

    [Test]
    public void ObservableRingBufferAddItem_AddEventRaised()
    {
        var buffer = new ObservableRingBuffer<int>(10);

        var eventRaised = false;
        buffer.CollectionChanged += (sender, args) =>
        {
            if (args.Action == NotifyCollectionChangedAction.Add)
            {
                eventRaised = true;
            }
        };

        buffer.Add(10);

        Assert.That(eventRaised);
    }

    [Test]
    public void ObservableRingBufferAddItem_ResetEventRaised()
    {
        var buffer = new ObservableRingBuffer<int>(10);

        var eventRaised = false;
        buffer.CollectionChanged += (sender, args) =>
        {
            if (args.Action == NotifyCollectionChangedAction.Reset)
            {
                eventRaised = true;
            }
        };

        buffer.Clear();

        Assert.That(eventRaised);
    }
}