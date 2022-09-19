using ArcticFoxEngine.Math;
using ImGuiNET;

namespace ArcticFoxEditor.EditorImGui;

public static class ImGuiExtensions
{
    // TODO: Check if math structs need [StructLayout(LayoutKind.Sequential)]

    // Misc. math structs.

    public unsafe static void Drag(string label, ref Point point)
    {
        fixed (Point* p = &point)
        {
            int* x = &p->x;
            ImGui.DragInt2(label, ref *x);
        }
    }

    public unsafe static void Drag(string label, ref Rectangle rectangle)
    {
        fixed (Rectangle* rect = &rectangle)
        {
            int* x = &rect->x;
            ImGui.DragInt4(label, ref *x);
        }
    }

    // Vectors

    public unsafe static void Drag(string label, ref Vector2 vector)
    {
        fixed (Vector2* vec = &vector)
        {
            var v = (System.Numerics.Vector2*)vec;
            ImGui.DragFloat2(label, ref *v);
        }
    }

    public unsafe static void Drag(string label, ref Vector2i vector)
    {
        fixed (Vector2i* vec = &vector)
        {
            int* x = &vec->x;
            ImGui.DragInt2(label, ref *x);
        }
    }

    public unsafe static void Drag(string label, ref Vector3 vector)
    {
        fixed (Vector3* vec = &vector)
        {
            var v = (System.Numerics.Vector3*)vec;
            ImGui.DragFloat3(label, ref *v);
        }
    }

    public unsafe static void Drag(string label, ref Vector3i vector)
    {
        fixed (Vector3i* vec = &vector)
        {
            int* x = &vec->x;
            ImGui.DragInt3(label, ref *x);
        }
    }

    public unsafe static void Drag(string label, ref Vector4 vector)
    {
        fixed (Vector4* vec = &vector)
        {
            var v = (System.Numerics.Vector4*)vec;
            ImGui.DragFloat4(label, ref *v);
        }
    }

    public unsafe static void Drag(string label, ref Vector4i vector)
    {
        fixed (Vector4i* vec = &vector)
        {
            int* x = &vec->x;
            ImGui.DragInt4(label, ref *x);
        }
    }
}
