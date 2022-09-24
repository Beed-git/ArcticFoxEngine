namespace ArcticFoxEditor.ProjectManagement;

public class ProjectModel
{
    public string Name { get; set; }

    public static ProjectModel Default => new()
    {
        Name = "Project",
    };
}
