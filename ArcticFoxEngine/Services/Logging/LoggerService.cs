namespace ArcticFoxEngine.Services.Logging;

public class LoggerService : ILogger
{
    public void Log(LogSeverity severity, string message)
    {
        Console.WriteLine(message); 
    }
}
