namespace ArcticFoxEngine.EC;

public class EntityManager
{
    private int _pointer;
    private readonly Queue<int> _freeIds;

    private readonly List<Entity> _entities;

    public EntityManager()
    {
        _freeIds = new Queue<int>();
        _entities = new List<Entity>();
    }

    private int GetId()
    {
        if (_freeIds.Count > 0)
        {
            return _freeIds.Dequeue();
        }
        return _pointer++;
    }

    public Entity CreateEntity()
    {
        var ent = new Entity(GetId());
        _entities.Add(ent);
        return ent;
    }

    public Entity CreateEntity(string name)
    {
        var ent = new Entity(GetId(), name);
        _entities.Add(ent);
        return ent;
    }

    public IEnumerable<Entity> CreateEntities(int amount)
    {
        var list = new List<Entity>();
        for (int i = 0; i < amount; i++)
        {
            var ent = new Entity(GetId());
            _entities.Add(ent);
            list.Add(ent);
        }
        return list;
    }

    public Entity? GetEntity(int id)
    {
        return _entities.Where(e => e.Id == id).FirstOrDefault(); 
    }

    public Entity? GetEntity(string name)
    {
        return _entities.Where(e => e.Name == name).FirstOrDefault();
    }

    public IEnumerable<Entity> GetEntities()
    {
        return _entities;
    }

    public void RemoveEntity(int id)
    {
        _entities.RemoveAll(e => e.Id == id);
    }

    public void RemoveEntity(string name)
    {
        _entities.RemoveAll(e => e.Name == name);
    }

    public void RemoveEntity(Entity entity)
    {
        _entities.Remove(entity);
    }
}
