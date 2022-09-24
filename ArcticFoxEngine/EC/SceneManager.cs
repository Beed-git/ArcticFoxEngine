namespace ArcticFoxEngine.EC;

internal class SceneManager
{
    private Scene? _currentScene;

    public SceneManager()
    {
    }

    public Scene? CurrentScene => _currentScene;

    public void ChangeScene(Scene scene)
    {
        _currentScene = scene;
        scene.Start();
    }

    public void Update(double dt)
    {
        _currentScene?.Update(dt);
    }

    public void Render(double dt)
    {
        _currentScene?.Render(dt);
    }
}
