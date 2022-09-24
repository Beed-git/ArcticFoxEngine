using ArcticFoxEditor;
using ArcticFoxEditor.ProjectManagement;

var projectManager = new ProjectManager();

var project = projectManager.LoadProject("Project1");

Console.WriteLine("ArcticFox Build Tool");
ConsoleKeyInfo key;
while (true)
{
    key = Console.ReadKey();
    if (key.Key is ConsoleKey.Escape)
    {
        break;
    }

    if (key.Key is ConsoleKey.B)
    {
        Console.WriteLine("Building Game");

        // TODO: Need to track which assets are actually used.

        var scripts = Directory.GetFiles(project.AssetsDir, "*.cs", SearchOption.AllDirectories);
        ProjectBuilder.Build(scripts, projectManager);

    }
}