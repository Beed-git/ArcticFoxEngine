namespace ArcticFoxEngine.Services.Window;

public interface IWindow
{
    public int Width { get; }
    public int Height { get; }

    // Integrate this render pipeling?
    // Renders 3d world, then 2d world on top.
    // Use interfaces or something maybe?

    //public event EventHandler<double> BeforeRender;

    //public event EventHandler<double> Before3DRender;
    //public event EventHandler<double> After3DRender;

    //public event EventHandler<double> BetweenRender;

    //public event EventHandler<double> Before2DRender;
    //public event EventHandler<double> After2DRender;

    //public event EventHandler<double> AfterRender;

    public void AddUpdateServices(IEnumerable<IUpdateService> updateServices);
    public void AddRenderServices(IEnumerable<IRenderService> renderServices);
    public void Close();
    public void Run();
}
