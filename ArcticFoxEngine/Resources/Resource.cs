namespace ArcticFoxEngine.Resources;

public class Resource<T> where T : class
{
    public Resource(string path)
    {
        Path = path;
        Data = null;
    }

    public Resource(string path, T? data)
    {
        Path = path;
        Data = data;
    }

    public string Path { get; private init; }
    public T? Data { get; set; }
}
