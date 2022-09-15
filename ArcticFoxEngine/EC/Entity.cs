using ArcticFoxEngine.Scripts;

namespace ArcticFoxEngine.EC;

public class Entity : IEntity
{
    private readonly HashSet<Component> _components;
    private readonly HashSet<BaseScript> _scripts;

    public Entity(int id) : this(id, $"Entity {id}")
    {
    }

    public Entity(int id, string name)
    {
        Id = id;
        Name = name;

        _components = new HashSet<Component>();
        _scripts = new HashSet<BaseScript>();
    }

    public string Name { get; set; }
    public int Id { get; private init; }
    public bool Started { get; set; }
    public IEnumerable<BaseScript> Scripts => _scripts;

    // Components

    public T? AddComponent<T>() where T : Component
    {
        var component = Activator.CreateInstance(typeof(T), this);
        if (component is T t)
        {
            _components.Add(t);
            return t;
        }
        else
        {
            return null;
        }
    }

    public bool HasComponent<T>() where T : Component
    {
        foreach (var component in _components)
        {
            if (component.GetType() == typeof(T))
            {
                return true;
            }
        }
        return false;
    }

    public T? GetComponent<T>() where T : Component
    {
        foreach (var component in _components)
        {
            if (component.GetType() == typeof(T))
            {
                return (T)component;
            }
        }
        return null;
    }

    public bool TryGetComponent<T>(out T value) where T : Component
    {
        foreach (var component in _components)
        {
            if (component.GetType() == typeof(T))
            {
                value = (T)component;
                return true;
            }
        }
        value = default;
        return false;
    }

    public IEnumerable<T> GetComponents<T>() where T : Component
    {
        var list = new List<T>();
        foreach (var component in _components)
        {
            if (component.GetType() == typeof(T))
            {
                list.Add((T)component);
            }
        }
        return list;
    }

    public void RemoveComponent<T>(T value) where T : Component
    {
        _components.Remove(value);
    }

    public void RemoveComponents<T>() where T : Component
    {
        _components.RemoveWhere(c => c is T);
    }

    // Scripts
    public void AddScript(BaseScript script) 
    {
        if (!_scripts.Contains(script))
        {
            _scripts.Add(script);
            if (Started)
            {
                script.OnCreate();
            }
        }
    }

    public void RemoveScript(BaseScript script)
    {
        _scripts.Remove(script);
    }
}
