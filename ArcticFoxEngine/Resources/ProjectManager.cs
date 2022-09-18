using System.Reflection;

namespace ArcticFoxEngine.Resources;

public class ProjectManager
{
    public ProjectManager(string directory, bool pathRelative = true)
    {
        if (pathRelative)
        {
            var location = Assembly.GetExecutingAssembly().Location;
            var parent = Directory.GetParent(location);
            if (parent is null)
            {
                throw new Exception("Failed to find engine folder.");
            }
            ProjectDirectory = Path.Combine(parent.FullName, directory);
        }
        else
        {
            ProjectDirectory = directory;
        }

        if (!Directory.Exists(ProjectDirectory))
        {
            Directory.CreateDirectory(ProjectDirectory);
        }

        AssetFolder = CreateRelativeFolder("assets");
        BuildFolder = CreateRelativeFolder("build");
    }
    
    public string ProjectDirectory { get; private init; }
    public string AssetFolder { get; private init; }
    public string BuildFolder { get; private init; }

    private string CreateRelativeFolder(string relativeDirectory)
    {
        var path = Path.Combine(ProjectDirectory, relativeDirectory);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        return path;
    }
}
