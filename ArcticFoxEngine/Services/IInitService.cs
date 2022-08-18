namespace ArcticFoxEngine.Services;

/// <summary>
/// Has an init method which is called after the services are injected
/// into the constructor and after all the InjectAttributes have been 
/// injected.
/// </summary>
public interface IInitService
{
    public void Init();
}
