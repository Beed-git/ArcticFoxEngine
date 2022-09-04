namespace ArcticFoxEngine.EC
{
    public interface IEntity
    {
        public string Name { get; set; }
        public void AddComponent<T>() where T : Component, new();
        public void AddComponent<T>(T value) where T : Component;
        
        public T? GetComponent<T>() where T : Component;
        public IEnumerable<T> GetComponents<T>() where T : Component;

        bool TryGetComponent<T>(out T? value) where T : Component;
        public bool HasComponent<T>() where T : Component;
        
        public void RemoveComponent<T>(T value) where T : Component;
        public void RemoveComponents<T>() where T : Component;
    }
}