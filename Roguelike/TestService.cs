using ArcticFoxEngine.Services;
using ArcticFoxEngine.Services.Logging;

namespace Roguelike;

internal class TestService : IUpdateService, IDisposable
{
    private readonly ILogger _logger;

    private int id;
    private static int counter = 0;

    public TestService(
        ILogger logger
    ) {
        _logger = logger;
        id = counter++;
    }

    public void Init()
    {
        _logger.Log("Called init on: " + id);
    }

    public void Update(int dt)
    {
        //_logger.Log("Update time: " + dt);
    }

    public void Dispose()
    {
        _logger.Log("Disposing of: " + id);
    }
}
