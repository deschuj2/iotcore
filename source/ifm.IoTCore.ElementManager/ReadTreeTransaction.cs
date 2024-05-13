namespace ifm.IoTCore.ElementManager;

using System;
using Contracts;

internal class ReadTreeTransaction : IReadTreeTransaction
{
    private readonly ElementManager _elementManager;

    public ReadTreeTransaction(ElementManager elementManager)
    {
        _elementManager = elementManager ?? throw new ArgumentNullException(nameof(elementManager));
    }

    public void Begin()
    {
        _elementManager.EnterReadLock();
    }

    public void End()
    {
        if (_elementManager.IsReadLockHeld)
        {
            _elementManager.ExitReadLock();
        }
    }
}