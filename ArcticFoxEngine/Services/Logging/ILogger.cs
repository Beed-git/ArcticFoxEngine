namespace ArcticFoxEngine.Services.Logging;

public enum LogSeverity
{
    Debug,
    Normal,
    Warning,
    Error,
    Critical,
}
public interface ILogger
{
    public void Log(LogSeverity severity, string message);

    public sealed void Log(string message)
    {
        Log(LogSeverity.Normal, message);
    }

    public sealed void Debug(string message)
    {
        Log(LogSeverity.Debug, message);
    }

    public sealed void Warn(string message)
    {
        Log(LogSeverity.Warning, message);
    }

    public sealed void Error(string message)
    {
        Log(LogSeverity.Error, message);
    }
}
