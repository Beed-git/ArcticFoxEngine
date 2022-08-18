namespace ArcticFoxEngine.Services.Window;

public interface IWindow
{
    public int Width { get; }
    public int Height { get; }
    public void AddRenderServices(IEnumerable<IRenderThreadService> renderServices);
    public void Close();
    public void Run();
}
