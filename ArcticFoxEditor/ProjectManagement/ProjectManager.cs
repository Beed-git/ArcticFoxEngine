using System.Reflection;

namespace ArcticFoxEditor.ProjectManagement;

public class ProjectManager
{
    public ProjectManager()
    {
        var location = Assembly.GetExecutingAssembly().Location;
        var parent = Directory.GetParent(location);
        if (parent is null)
        {
            throw new Exception("Failed to find engine folder.");
        }

        RootDirectory = parent.FullName;
        ProjectsDirectory = Path.Combine(RootDirectory, "projects");
        Directory.CreateDirectory(ProjectsDirectory);
    }

    public Project CurrentProject { get; private set; }
    public string RootDirectory { get; private init; }
    public string ProjectsDirectory { get; private init; }

    public Project LoadProject(string name)
    {
        var path = Path.Combine(ProjectsDirectory, name);
        var project = new Project(path);
        CurrentProject = project;
        return project;
    }
}
