namespace ArcticFoxEngine.Logging;

public interface ILogger
{
    public void Log(string message);
    public void Warn(string message);
    public void Error(string message);
}
