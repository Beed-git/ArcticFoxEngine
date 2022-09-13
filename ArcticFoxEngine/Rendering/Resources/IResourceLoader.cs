﻿namespace ArcticFoxEngine.Rendering.Resources;

public interface IResourceLoader
{
}

public interface IResourceLoader<T> : IResourceLoader
{
    public T? LoadResource(string path);
}
