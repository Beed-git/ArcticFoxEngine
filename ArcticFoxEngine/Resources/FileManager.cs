using System.Reflection;

namespace ArcticFoxEngine.Resources;

internal class FileManager
{
    public FileManager()
    {
        var location = Assembly.GetExecutingAssembly().Location;
        var parent = Directory.GetParent(location);
        if (parent is null)
        {
            throw new Exception("Failed to find engine folder.");
        }

        ProjectDirectory = parent.FullName;
        AssetFolder = CreateRelativeFolder("assets");
    }
    
    public string ProjectDirectory { get; private init; }
    public string AssetFolder { get; private init; }

    private string CreateRelativeFolder(string relativeDirectory)
    {
        var path = Path.Combine(ProjectDirectory, relativeDirectory);
        Directory.CreateDirectory(path);
        return path;
    }
}
