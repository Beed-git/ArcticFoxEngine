using ArcticFoxEngine.EC.Models;
using ArcticFoxEngine.Scripts;

namespace ArcticFoxEngine.EC;

public class Entity : IEntity
{
    private readonly HashSet<IComponentModel> _components;
    private readonly Dictionary<string, BaseScript> _scripts;

    public Entity(int id) : this(id, $"Entity {id}")
    {
    }

    public Entity(int id, string name)
    {
        Id = id;
        Name = name;

        _components = new HashSet<IComponentModel>();
        _scripts = new Dictionary<string, BaseScript>();
    }

    public string Name { get; set; }
    public int Id { get; private init; }
    public bool Started { get; set; }
    public Dictionary<string, BaseScript> Scripts => _scripts;

    // Components

    internal IEnumerable<IComponentModel> GetAllComponents()
    {
        return _components;
    }

    public T? AddComponent<T>() where T : class, IComponentModel
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
    public void AddComponent<T>(T component) where T : class, IComponentModel
    {
        _components.Add(component);
    }

    public bool HasComponent<T>() where T : class, IComponentModel
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

    public T? GetComponent<T>() where T : class, IComponentModel
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

    public bool TryGetComponent<T>(out T value) where T : class, IComponentModel
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

    public IEnumerable<T> GetComponents<T>() where T : class, IComponentModel
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

    public void RemoveComponent<T>(T value) where T : class, IComponentModel
    {
        _components.Remove(value);
    }

    public void RemoveComponents<T>() where T : class, IComponentModel
    {
        _components.RemoveWhere(c => c is T);
    }

    // Scripts
    public void AddScript<T>(T script) where T : BaseScript
    {
        var type = script.GetType();
        var key = type.FullName;
        if (!_scripts.ContainsKey(key))
        {
            _scripts.Add(key, script);
            if (Started)
            {
                script.OnCreate();
            }
        }
    }

    public void RemoveScript<T>() where T : BaseScript
    {
        var key = typeof(T).Name;
        _scripts.Remove(key);
    }
}
