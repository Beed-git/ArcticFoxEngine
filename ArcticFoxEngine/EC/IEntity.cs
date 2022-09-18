using ArcticFoxEngine.Components;

namespace ArcticFoxEngine.EC
{
    public interface IEntity
    {
        public string Name { get; set; }
        public T? AddComponent<T>() where T : ComponentModel;
        
        public T? GetComponent<T>() where T : ComponentModel;
        public IEnumerable<T> GetComponents<T>() where T : ComponentModel;

        bool TryGetComponent<T>(out T value) where T : ComponentModel;
        public bool HasComponent<T>() where T : ComponentModel;
        
        public void RemoveComponent<T>(T value) where T : ComponentModel;
        public void RemoveComponents<T>() where T : ComponentModel;
    }
}