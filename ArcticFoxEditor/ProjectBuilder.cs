using ArcticFoxEditor.ProjectManagement;
using Basic.Reference.Assemblies;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Text;

namespace ArcticFoxEditor;

public static class ProjectBuilder
{
    private static string CreateExeCode(string[] paths)
    {
        var stringBuilder = new StringBuilder(@"
using ArcticFoxEngine.Rendering;
using System.Reflection;");

        if (paths.Length > 0)
        {
            stringBuilder.Append(@"
var assemblies = new Assembly[]
{");
            foreach (var path in paths)
            {
                stringBuilder.Append("  Assembly.LoadFile(@\""); 
                stringBuilder.Append(path);
                stringBuilder.Append("\"),\n"); 
            }
            stringBuilder.Append(@"
};
using var window = new GameWindow(WindowSettings.Default, assemblies);
window.Run();
");
        }
        else
        {
            stringBuilder.Append(@"
using var window = new GameWindow(WindowSettings.Default, null);
window.Run();
");
        }
        return stringBuilder.ToString();
    }

    public static void Build(IEnumerable<string> scripts, ProjectManager projectManager)
    {
        var name = projectManager.CurrentProject.ProjectInfo.Name;
        var outputDir = projectManager.CurrentProject.BuildDir;
        var engineProjectPath = Path.Combine(projectManager.RootDirectory, "ArcticFoxEngine.dll");
        var enginePath = Path.Combine(projectManager.CurrentProject.BuildDir, "ArcticFoxEngine.dll");

        if (File.Exists(enginePath))
        {
            File.Delete(enginePath);
        }
        File.Copy(engineProjectPath, enginePath);

        var references = ReferenceAssemblies.Net60.ToList();
        references.Add(MetadataReference.CreateFromFile(enginePath));


        var dllPath = BuildDll(name, outputDir, scripts, references);
        if (dllPath is null)
        {
            return;
        }

        references.Add(MetadataReference.CreateFromFile(dllPath));
        var code = CreateExeCode(new string[] { dllPath });
        BuildExe(name, outputDir, references, code);
    }
    private static string? BuildDll(string name, string path, IEnumerable<string> scripts, IEnumerable<MetadataReference> references)
    { 
        var trees = new List<SyntaxTree>();
        foreach (var script in scripts)
        {
            var code = File.ReadAllText(script);
            var tree = SyntaxFactory.ParseSyntaxTree(code);
            trees.Add(tree);
        }

        var compiler = CSharpCompilation.Create(name)
            .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, optimizationLevel: OptimizationLevel.Release))
            .WithReferences(references)
            .AddSyntaxTrees(trees); 

        var outputPath = Path.ChangeExtension(Path.Combine(path, name), "dll");
        if (File.Exists(outputPath))
        {
            File.Delete(outputPath);
        }
        using var stream = new FileStream(outputPath, FileMode.CreateNew);

        var result = compiler.Emit(stream);
        if (result.Success)
        {
            stream.Close();
            Console.WriteLine("Game DLL compiled.");
            return outputPath;
        }
        else
        {
            Console.WriteLine("Failed to compile DLL!");
            foreach (var error in result.Diagnostics)
            {
                Console.WriteLine(error.ToString());
            }
            return null;
        }
    }

    private static void BuildExe(string name, string path, IEnumerable<MetadataReference> references, string programCode)
    {
        var trees = new List<SyntaxTree> { SyntaxFactory.ParseSyntaxTree(programCode) };

        var compiler = CSharpCompilation.Create(name)
            .WithOptions(new CSharpCompilationOptions(OutputKind.ConsoleApplication, optimizationLevel: OptimizationLevel.Release))
            .WithReferences(references)
            .AddSyntaxTrees(trees);

        var outputPath = Path.ChangeExtension(Path.Combine(path, name), "exe");
        if (File.Exists(outputPath))
        {
            File.Delete(outputPath);
        }

        using var stream = new FileStream(outputPath, FileMode.CreateNew);

        var result = compiler.Emit(stream);
        if (result.Success)
        {
            stream.Close();
            Console.WriteLine("Game exe compiled.");
        }
        else
        {
            Console.WriteLine("Failed to compile exe!");
            foreach (var error in result.Diagnostics)
            {
                Console.WriteLine(error.ToString());
            }
        }
    }
}
