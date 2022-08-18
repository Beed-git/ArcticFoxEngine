using System.Diagnostics;

namespace ArcticFoxEngine.Services.Game;

public class GameManager : IGameManager, IDisposable
{
    private readonly HashSet<IUpdateThreadService> _updateServices;
    private Thread? _thread;

    public GameManager() 
    {
        _updateServices = new HashSet<IUpdateThreadService>();
    }

    public bool Running { get; set; }

    public void AddUpdateServices(IEnumerable<IUpdateThreadService> updateServices)
    {
        foreach (var service in updateServices)
        {
            _updateServices.Add(service);
        }
    }

    public void Run()
    {
        if (_thread is not null)
        {
            throw new Exception("Game manager is already running a game thread!");
        }
        _thread = new Thread(RunThread);
        _thread.Start();
    }

    private void RunThread()
    {
        var watch = new Stopwatch();
        watch.Start();
        Running = true;

        double lastTime = watch.Elapsed.TotalMilliseconds;
        while (Running)
        {
            double currentTime = watch.Elapsed.TotalMilliseconds;
            double dt = currentTime - lastTime;

            foreach (var service in _updateServices)
            {
                service.Update(dt);
            }

            lastTime = currentTime;
        }
    }

    public void Dispose()
    {
        Running = false;
        _thread?.Join();
        _thread = null;
    }
}
