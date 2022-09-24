using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace ArcticFoxEditor.ProjectManagement;

public class Project
{
    public Project(string directory)
    {
        ProjectDir = directory;

        AssetsDir = CreateRelativeFolder("assets");
        BuildDir = CreateRelativeFolder("build");

        ProjectInfo = GetProjectModel();
    }

    public ProjectModel ProjectInfo { get; private set; }
    public string ProjectDir { get; private init; }
    public string AssetsDir { get; private init; }
    public string BuildDir { get; private init; }

    private string CreateRelativeFolder(string relativeDirectory)
    {
        var path = Path.Combine(ProjectDir, relativeDirectory);
        Directory.CreateDirectory(path);
        return path;
    }

    private ProjectModel GetProjectModel()
    {
        var serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .DisableAliases()
            .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull)
            .Build();

        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        var path = Path.Combine(ProjectDir, "project.yml");

        if (File.Exists(path))
        {
            return deserializer.Deserialize<ProjectModel>(File.ReadAllText(path));
        }
        
        var model = ProjectModel.Default;
        File.WriteAllText(path, serializer.Serialize(model));
        return model;
    }
}
