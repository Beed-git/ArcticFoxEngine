using Microsoft.Extensions.DependencyInjection;

namespace ArcticFoxEngine.Services.Window;

public static class WindowExtensions
{
    public static IServiceCollection AddWindow<TWindowEventHandler>(this IServiceCollection services)
        where TWindowEventHandler : class, IWindowEventHandler
    {
        return services
            .AddSingleton<IWindow, Window>()
            .AddSingleton<IWindowEventHandler, TWindowEventHandler>();
    }
}
