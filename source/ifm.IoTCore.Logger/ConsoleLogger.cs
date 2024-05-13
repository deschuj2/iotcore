namespace ifm.IoTCore.Logger;

using System;
using Contracts;

public class ConsoleLogger : ILogger
{
    public void Info(string message)
    {
        Console.WriteLine($"Info: {message}");
    }

    public void Warning(string message)
    {
        Console.WriteLine($"Warning: {message}");
    }

    public void Error(string message)
    {
        Console.WriteLine($"Error: {message}");
    }

    public void Debug(string message)
    {
        Console.WriteLine($"Debug: {message}");
    }
}