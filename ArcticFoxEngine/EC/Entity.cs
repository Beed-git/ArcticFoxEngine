using ArcticFoxEngine.Scripts;

namespace ArcticFoxEngine.EC;

public class Entity : IEntity
{
    private readonly List<Component> _components;
    private readonly List<BaseScript> _scripts;

    public Entity(int id) : this(id, $"Entity {id}")
    {
    }

    public Entity(int id, string name)
    {
        Id = id;
        Name = name;

        _components = new List<Component>();
        _scripts = new List<BaseScript>();

    }

    public string Name { get; set; }
    public int Id { get; private init; }
    public bool Started { get; set; }
    public IEnumerable<BaseScript> Scripts => _scripts;

    // Components

    public void AddComponent<T>() where T : Component, new()
    {
        var component = Activator.CreateInstance<T>();
        _components.Add(component);
    }

    public void AddComponent<T>(T value) where T : Component
    {
        _components.Add(value);
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

    public bool TryGetComponent<T>(out T? value) where T : Component
    {
        foreach (var component in _components)
        {
            if (component.GetType() == typeof(T))
            {
                value = (T)component;
                return true;
            }
        }
        value = null;
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
        for (int i = _components.Count - 1; i > 0; i--)
        {
            if (_components[i].GetType() == typeof(T))
            {
                _components.RemoveAt(i);
            }
        }
    }

    // Scripts

    public void AttachScript<T>() where T : BaseScript
    {
        var script = (T?)Activator.CreateInstance(typeof(T), this);
        if (script is not null)
        {
            if (Started)
            {
                script.OnCreate();
            }
            _scripts.Add(script);
        }
        else
        {
            throw new Exception($"Failed to instantiate script of type '{typeof(T).Name}'");
        }
    }

    public void DetachScripts<T>() where T : BaseScript
    {
        _scripts.RemoveAll(t => t.GetType() == typeof(T));
    }

    public void DetachScript<T>(T value) where T : BaseScript
    {
        _scripts.Remove(value);
    }
}
