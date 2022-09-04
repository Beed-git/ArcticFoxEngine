namespace ArcticFoxEngine.EC;

public class SceneManager
{
    private Scene? _currentScene;

    public SceneManager()
    {
    }

    public void ChangeScene(Scene scene)
    {
        _currentScene = scene;
        scene.Start();
    }

    public void Update(double dt)
    {
        _currentScene?.Update(dt);
    }

    public void FixedUpdate(double dt)
    {
        _currentScene?.FixedUpdate(dt);
    }
}
