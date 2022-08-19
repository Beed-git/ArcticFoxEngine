using Microsoft.Extensions.DependencyInjection;

namespace ArcticFoxEngine.Services.TextureManager;

public static class TextureManagerExtensions
{
    public static IServiceCollection AddTextureManager(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddSingleton<ITextureManager, TextureManagerService>();
    }
}
