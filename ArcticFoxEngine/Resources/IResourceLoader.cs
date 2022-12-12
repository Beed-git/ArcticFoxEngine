namespace ArcticFoxEngine.Resources;

internal interface IResourceLoader
{
}

internal interface IResourceLoader<T> : IResourceLoader
{
    public T? LoadResource(FileManager fileManager, string path);
}
