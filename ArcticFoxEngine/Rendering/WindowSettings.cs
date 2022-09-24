using ArcticFoxEngine.Math;

namespace ArcticFoxEngine.Rendering;

internal struct WindowSettings
{
    public string Title { get; set; }
    public Vector2i Size { get; set; }

    public static WindowSettings Default => new()
    { 
        Title = "ArcticFox Engine",
        Size = new Vector2i(1920, 1080),
    };
}
