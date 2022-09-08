using ArcticFoxEngine.Math;
using ImGuiNET;
using Silk.NET.Vulkan;

namespace ArcticFoxEngine.Rendering;

public static class ImGuiExtensions
{
    public unsafe static void DragInt2(string label, ref Vector2i vector)
    {
        fixed (Vector2i* vec = &vector)
        {
            int* x = &vec->x;
            ImGui.DragInt2(label, ref *x);
        }
    }

    public unsafe static void DragInt3(string label, ref Vector3i vector)
    {
        fixed (Vector3i* vec = &vector)
        {
            int* x = &vec->x;
            ImGui.DragInt3(label, ref *x);
        }
    }

    public unsafe static void DragInt4(string label, ref Vector4i vector)
    {
        fixed (Vector4i* vec = &vector)
        {
            int* x = &vec->x;
            ImGui.DragInt4(label, ref *x);
        }
    }
}
