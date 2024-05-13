namespace ifm.IoTCore.Logger;

using Contracts;

public class NullLogger : ILogger
{
    public void Info(string message)
    {
    }

    public void Warning(string message)
    {
    }

    public void Error(string message)
    {
    }

    public void Debug(string message)
    {
    }
}