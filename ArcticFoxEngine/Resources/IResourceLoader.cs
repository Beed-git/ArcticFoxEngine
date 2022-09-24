namespace ArcticFoxEngine.Resources;

internal interface IResourceLoader
{
}

internal interface IResourceLoader<T> : IResourceLoader
{
    public T? LoadResource(string path);
}
