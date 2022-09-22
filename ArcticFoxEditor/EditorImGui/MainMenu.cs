using ArcticFoxEngine;
using ImGuiNET;

namespace ArcticFoxEditor.EditorImGui;

public class MainMenu
{
    private readonly Core _core;

    public MainMenu(Core core)
    {
        _core = core;
    }

    public void Draw()
    {
        ImGui.BeginMainMenuBar();

        if (ImGui.BeginMenuBar())
        {
            if (ImGui.BeginMenu("File"))
            {
                ImGui.EndMenu();
            }
            if (ImGui.MenuItem("Run"))
            {
                RunGame();
            }
            ImGui.EndMenuBar();
        }

        ImGui.EndMainMenuBar();
    }

    private void BuildGame()
    {
        Directory.GetFiles()
    }

    private void RunGame()
    {
        BuildGame();
    }
}
