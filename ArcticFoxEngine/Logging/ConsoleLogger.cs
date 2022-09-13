namespace ArcticFoxEngine.Logging;

public class ConsoleLogger : ILogger
{
    public void Log(string message)
    {
        Console.WriteLine(message);
    }

    public void Warn(string message)
    {
        Console.WriteLine($"[WARN] {message}");
    }
}
