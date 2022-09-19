using ArcticFoxEngine.EC.Models;
using ArcticFoxEngine.Rendering;
using ArcticFoxEngine.Rendering.Camera;
using ArcticFoxEngine.Resources;
using ArcticFoxEngine.Scripts;

namespace ArcticFoxEngine.EC.Converters;

public static class SceneModelConverter
{
    private static EntityModel ToEntityModel(Entity entity)
    {
        var model = new EntityModel();

        var entComponents = entity.GetAllComponents();
        if (entComponents.Any())
        {
            var components = new List<IComponentModel>();
            foreach (var component in entComponents)
            {
                components.Add(component);
            }
            model.Components = components;
        }

        var entScripts = entity.Scripts.Keys;
        if (entScripts.Any())
        {
            var scripts = new List<string>();
            foreach (var script in entScripts)
            {
                scripts.Add(script);
            }
            model.Scripts = scripts;
        }

        return model;
    }

    public static SceneModel ToModel(Scene scene)
    {
        var entities = new Dictionary<string, EntityModel>();
        foreach (var ent in scene.EntityManager.GetEntities())
        {
            var model = ToEntityModel(ent);
            entities.Add(ent.Name, model);
        }

        return new SceneModel()
        {
            Name = scene.Name,
            Camera = scene.MainCamera.GetType().Name,
            BackgroundColor = scene.BackgroundColor,
            Entities = entities
        };
    }

    public static Scene ToScene(GraphicsDevice graphicsDevice, ResourceManager resourceManager, SceneModel model)
    {
        var scene = new Scene(graphicsDevice, resourceManager)
        {
            Name = model.Name,
            MainCamera = GetCamera(model.Camera),
            BackgroundColor = model.BackgroundColor,
        };

        foreach (var (name, entModel) in model.Entities)
        {
            var ent = scene.CreateEntity(name);
            if (entModel.Components is not null)
            {
                // Since components are just a set of fields/properties, the model maps to the actual component.
                // Therefor we don't need to do any special transformations.
                foreach (var component in entModel.Components)
                {
                    ent.AddComponent(component);
                }
            }
            if (entModel.Scripts is not null)
            {
                foreach (var script in entModel.Scripts)
                {
                    //TODO: Not sure how scripts are going to be handled yet.
                    Console.WriteLine($"Entity '{name}' has script '{script}'");
                    var factory = resourceManager.GetResource<ScriptFactory>(script);
                    if (factory.Data is not null)
                    { 
                        ent.AddScript(factory.Data.CreateScript(ent));
                    }
                }
            }
        }

        return scene;
    }

    private static Camera2D GetCamera(string cameraName)
    {
        Console.WriteLine("Currently only support camera2d.");
        return new Camera2D();
    }
}
