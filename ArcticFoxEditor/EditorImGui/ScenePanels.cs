using ArcticFoxEngine;
using ArcticFoxEngine.EC;
using ArcticFoxEngine.Math;
using ImGuiNET;

namespace ArcticFoxEditor.EditorImGui;

public class ScenePanels
{
    private readonly Core _core;
    private readonly EnumerableListBox<Entity> _entities;

    public ScenePanels(Core core)
    {
        _core = core;
        _entities = new EnumerableListBox<Entity>();
    }

    public void DrawEntityPanel()
    {
        ImGui.Begin("Entities");

        // TODO: Scene selector.
        var scene = _core.CurrentScene;
        ImGui.BeginChild("SceneScroll");
        
        ImGui.Selectable(scene.Name, true);

        if (scene is not null)
        {
            ImGui.Indent();

            var entites = scene.EntityManager.GetEntities();
            _entities.Draw("EntityScroll", entites);

            ImGui.Unindent();
        }

        ImGui.EndChild();
        ImGui.End();

    }

    public void DrawPropertiesPanel()
    {
        ImGui.Begin("Properties");

        if (_entities.SelectedValue is Entity entity)
        {
            ImGui.Text("[Entity]");
            ImGui.Text($"{entity}");
            ImGui.NewLine();

            ImGui.Text("[Components]");
            foreach (var component in entity.GetAllComponents())
            {
                ImGui.Text(component.GetType().Name);
                foreach (var property in component.GetType().GetProperties())
                {
                    var value = property.GetValue(component);
                    if (value is not null)
                    {
                        switch (value)
                        {
                            case string s:
                                ImGui.InputText(property.Name, ref s, 256);
                                property.SetValue(component, value);
                                break;
                            case double d:
                                ImGui.InputDouble(property.Name, ref d);
                                property.SetValue(component, d);
                                break;
                            case int i: 
                                ImGui.DragInt(property.Name, ref i); 
                                property.SetValue(component, i); 
                                break;
                            case float f:
                                ImGui.DragFloat(property.Name, ref f);
                                property.SetValue(component, f);
                                break;
                            case bool b:
                                ImGui.Checkbox(property.Name, ref b);
                                property.SetValue(component, b);
                                break;
                            case Color col:
                                var c = (System.Numerics.Vector4)Color.ToVector4(col);
                                ImGui.ColorEdit4(property.Name, ref c);
                                property.SetValue(component, new Color(c));
                                break;
                            case Point p: 
                                ImGuiExtensions.Drag(property.Name, ref p); 
                                property.SetValue(component, p); 
                                break;
                            case Rectangle rect: 
                                ImGuiExtensions.Drag(property.Name, ref rect); 
                                property.SetValue(component, rect); 
                                break;
                            case Vector2 v2: 
                                ImGuiExtensions.Drag(property.Name, ref v2); 
                                property.SetValue(component, v2); 
                                break;
                            case Vector2i v2i: 
                                ImGuiExtensions.Drag(property.Name, ref v2i); 
                                property.SetValue(component, v2i); 
                                break;
                            case Vector3 v3: 
                                ImGuiExtensions.Drag(property.Name, ref v3); 
                                property.SetValue(component, v3); 
                                break;
                            case Vector3i v3i: 
                                ImGuiExtensions.Drag(property.Name, ref v3i); 
                                property.SetValue(component, v3i); 
                                break;
                            case Vector4 v4: 
                                ImGuiExtensions.Drag(property.Name, ref v4); 
                                property.SetValue(component, v4); 
                                break;
                            case Vector4i v4i: 
                                ImGuiExtensions.Drag(property.Name, ref v4i); 
                                property.SetValue(component, v4i); 
                                break;
                            default: ImGui.Text(value.ToString()); break;
                        }
                    }
                }
            }
            // ImGuiExtensions.DragInt2("Transform", ref i);
            //var v = new System.Numerics.Vector3();
            //ImGui.DragFloat3("s", ref v);
            }
        
        ImGui.End();
    }
}
