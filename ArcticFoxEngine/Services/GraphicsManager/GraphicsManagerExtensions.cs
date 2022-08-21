using Microsoft.Extensions.DependencyInjection;
using OpenTK.Windowing.Desktop;

namespace ArcticFoxEngine.Services.GraphicsManager;

public static class GraphicsManagerExtensions
{
    private class Window : GameWindow
    {
        public Window() : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
        }

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
        }
    }

    public static IServiceCollection AddGraphicsManager(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddSingleton<GameWindow, Window>()
            .AddSingleton<IGraphicsManager, GraphicsManagerService>();
    }
}
