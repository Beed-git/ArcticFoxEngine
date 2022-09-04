using ArcticFoxEngine.EC;
using System.Diagnostics;

namespace ArcticFoxEngine;

public class Core : IDisposable
{
    private readonly Stopwatch _updateWatch;
    private readonly Stopwatch _fixedUpdateWatch;
    private readonly Stopwatch _renderWatch;

    private readonly double _fixedUpdateFrequency;
    private readonly double _renderFrequency;

    private readonly SceneManager _sceneManager;

    public Core()
    {
        _updateWatch = new Stopwatch();
        _fixedUpdateWatch = new Stopwatch();
        _renderWatch = new Stopwatch();

        _fixedUpdateFrequency = 1.0f / 60;
        _renderFrequency = 1.0f / 60;

        _sceneManager = new SceneManager();
    }

    public bool Running { get; set; }

    public void Run()
    {
        Running = true;

        _updateWatch.Start();
        _fixedUpdateWatch.Start();
        _renderWatch.Start();

        while (Running)
        {
            Update();
            FixedUpdate();
            Render();
        }

    }

    private void Update()
    {
        var dt = _updateWatch.Elapsed.TotalSeconds;
        _updateWatch.Restart();
        _sceneManager.Update(dt);
    }

    private void FixedUpdate()
    {
        var dt = _fixedUpdateWatch.Elapsed.TotalSeconds;

        if (_fixedUpdateWatch.Elapsed.TotalSeconds > _fixedUpdateFrequency)
        {
            _fixedUpdateWatch.Restart();
            _sceneManager.FixedUpdate(dt);
        }
    }
    
    private void Render()
    {
        var dt = _renderWatch.Elapsed.TotalSeconds;

        if (_renderWatch.Elapsed.TotalSeconds > _renderFrequency)
        {
            _renderWatch.Restart();
        }
    }

    public void Dispose()
    {
    }
}
