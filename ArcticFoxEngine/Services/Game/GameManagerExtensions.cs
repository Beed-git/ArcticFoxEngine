using Microsoft.Extensions.DependencyInjection;

namespace ArcticFoxEngine.Services.Game;

public static class GameManagerExtensions
{
    public static IServiceCollection AddGameManager(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddSingleton<IGameManager, GameManager>();
    }
}
