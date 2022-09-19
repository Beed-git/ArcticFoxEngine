using ArcticFoxEngine.EC;
using ImGuiNET;

namespace ArcticFoxEditor.EditorImGui;

public class EnumerableListBox<T> where T : class
{
    public EnumerableListBox()
    {
        SelectedValue = null;
    }

    public T? SelectedValue { get; private set; }

    public void Draw(string id, IEnumerable<T> values)
    {
        ImGui.BeginChild(id);

        int i = 0;
        foreach (var value in values)
        {
            ImGui.PushID(i);
            if (ImGui.Selectable(value.ToString(), SelectedValue == value))
            {
                SelectedValue = value;
            }
            ImGui.PopID();
            i++;
        }

        ImGui.EndChild();
    }
}
