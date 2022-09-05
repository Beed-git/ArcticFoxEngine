using ArcticFoxEngine.EC;
using ArcticFoxEngine.Math;

namespace ArcticFoxEngine;

public class Core
{
    private readonly SceneManager _sceneManager;

    public Core()
    {
        _sceneManager = new SceneManager();
    }

    public void OnLoad()
    {
        _sceneManager.ChangeScene(new Scene());
    }

    public void OnUpdate(double dt)
    {
        _sceneManager.Update(dt);
    }

    public void OnRender(double dt)
    {
    }

    public void OnResize(Vector2i size)
    {
    }
}
