using System;
using Application.Abstractions;

namespace Infrastructure.Logging;

public sealed class ConsoleAppLogger : IAppLogger
{
    public void Info(string message)
    {
        Console.WriteLine($"[INFO] {DateTime.UtcNow:O} - {message}");
    }

    public void Error(string message, Exception exception)
    {
        Console.Error.WriteLine($"[ERROR] {DateTime.UtcNow:O} - {message}: {exception}");
    }
}
