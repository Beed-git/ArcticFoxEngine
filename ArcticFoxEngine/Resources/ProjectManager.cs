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

        AssetFolder = Path.Combine(ProjectDirectory, "assets");
    }
    
    public string ProjectDirectory { get; private init; }
    public string AssetFolder { get; private init; }
}
