namespace ArcticFoxEngine.EC;

public class Scene
{
    private readonly EntityManager _entityManager;
    private bool _started;

    public Scene()
    {
        _entityManager = new EntityManager();
        _started = false;
    }

    public void Start()
    {
        _started = true;
        foreach (var ent in _entityManager.GetEntities())
        {
            foreach (var script in ent.Scripts)
            {
                script.OnCreate();
            }
        }
    }

    public void Update(double dt)
    {
        foreach (var ent in _entityManager.GetEntities())
        {
            foreach (var script in ent.Scripts)
            {
                script.OnUpdate(dt);
            }
        }
    }

    public Entity CreateEntity()
    {
        var ent = _entityManager.CreateEntity();
        ent.Started = _started;
        return ent;
    }

    public void RemoveEntity(Entity entity)
    {
        _entityManager.RemoveEntity(entity);
    }
}
