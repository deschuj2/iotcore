namespace ifm.IoTCore.Common.UnitTests;

using NUnit.Framework;

public class DisposableDerived : DisposableBase
{
    public new bool IsDisposed => base.IsDisposed;

    public new void Dispose(bool disposing)
    {
        base.Dispose(disposing);
    }
}

[TestFixture]
public class DisposableBaseTests
{
    [Test]
    public void DisposeDerived_ObjectIsDisposed()
    {
        var o = new DisposableDerived();
        o.Dispose();
        o.Dispose(false);
        Assert.That(o.IsDisposed);

        // Check that object is removed from finalizer queue
    }
}