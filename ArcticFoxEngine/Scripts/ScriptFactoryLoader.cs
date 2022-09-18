using ArcticFoxEngine.Logging;
using ArcticFoxEngine.Resources;
using Basic.Reference.Assemblies;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;

namespace ArcticFoxEngine.Scripts;

public class ScriptFactoryLoader : IResourceLoader<ScriptFactory>
{
    private readonly ILogger _logger;

    public ScriptFactoryLoader(ILogger logger)
    {
        _logger = logger;
    }

    public ScriptFactory? LoadResource(string path)
    {
        if (!File.Exists(path))
        {
            return null;
        }
        var code = File.ReadAllText(path);

        var references = ReferenceAssemblies.Net60.ToList();
        references.Add(MetadataReference.CreateFromFile(Assembly.GetExecutingAssembly().Location));

        var tree = SyntaxFactory.ParseSyntaxTree(code.Trim());
        var compiler = CSharpCompilation.Create("test.cs")
            .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, optimizationLevel: OptimizationLevel.Release))
            .WithReferences(references)
            .AddSyntaxTrees(tree);

        using var stream = new MemoryStream();

        var result = compiler.Emit(stream);
        if (result.Success)
        {
            var assembly = Assembly.Load(stream.ToArray());
            foreach (var type in assembly.GetTypes())
            {
                if (type.IsAssignableTo(typeof(BaseScript)))
                {
                    return new ScriptFactory(type);
                }
            }
            _logger.Error("Failed to find a class which implements BaseScript!");
            return null;
        }
        else
        {
            _logger.Log("Failed to compile!");
            foreach (var error in result.Diagnostics)
            {
                _logger.Error(error.ToString());
            }
            return null;
        }
    }
}
