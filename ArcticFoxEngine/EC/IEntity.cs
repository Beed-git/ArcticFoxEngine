using ArcticFoxEngine.EC.Models;

namespace ArcticFoxEngine.EC
{
    public interface IEntity
    {
        public string Name { get; set; }
        public T? AddComponent<T>() where T : class, IComponentModel;
        
        public T? GetComponent<T>() where T : class, IComponentModel;
        public IEnumerable<T> GetComponents<T>() where T : class, IComponentModel;

        bool TryGetComponent<T>(out T value) where T : class, IComponentModel;
        public bool HasComponent<T>() where T : class, IComponentModel;
        
        public void RemoveComponent<T>(T value) where T : class, IComponentModel;
        public void RemoveComponents<T>() where T : class, IComponentModel;
    }
}