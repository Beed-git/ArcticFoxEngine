using ArcticFoxEngine.Components;
using ArcticFoxEngine.Scripts;

namespace ArcticFoxEngine.EC;

public class Entity : IEntity
{
    private readonly HashSet<ComponentModel> _components;
    private readonly HashSet<BaseScript> _scripts;

    public Entity(int id) : this(id, $"Entity {id}")
    {
    }

    public Entity(int id, string name)
    {
        Id = id;
        Name = name;

        _components = new HashSet<ComponentModel>();
        _scripts = new HashSet<BaseScript>();
    }

    public string Name { get; set; }
    public int Id { get; private init; }
    public bool Started { get; set; }
    public IEnumerable<BaseScript> Scripts => _scripts;

    // Components

    public T? AddComponent<T>() where T : ComponentModel
    {
        var component = Activator.CreateInstance<T>();
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

    //TODO: Unexpected behaviour can occur as we are passing around a reference to the component.
    public void AddComponent<T>(T component) where T : ComponentModel
    {
        _components.Add(component);
    }

    public bool HasComponent<T>() where T : ComponentModel
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

    public T? GetComponent<T>() where T : ComponentModel
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

    public bool TryGetComponent<T>(out T value) where T : ComponentModel
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

    public IEnumerable<T> GetComponents<T>() where T : ComponentModel
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

    public void RemoveComponent<T>(T value) where T : ComponentModel
    {
        _components.Remove(value);
    }

    public void RemoveComponents<T>() where T : ComponentModel
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
