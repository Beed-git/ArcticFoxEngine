using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace ArcticFoxEngine.Rendering.OpenGL;

public class Shader : IDisposable
{
    private readonly int _handle;
    private readonly Dictionary<string, int> _uniforms;

    public Shader(string vert, string frag)
    {
        var vertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexShader, vert);
        CompileShader(vertexShader);

        var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentShader, frag);
        CompileShader(fragmentShader);

        _handle = GL.CreateProgram();
        GLUtil.CheckError();

        GL.AttachShader(_handle, vertexShader);
        GL.AttachShader(_handle, fragmentShader);

        LinkProgram(_handle);
        GL.GetProgram(_handle, GetProgramParameterName.LinkStatus, out var linkStatus);

        GL.DetachShader(_handle, vertexShader);
        GL.DetachShader(_handle, fragmentShader);
        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);

        _uniforms = new Dictionary<string, int>();
        GL.GetProgram(_handle, GetProgramParameterName.ActiveUniforms, out var uniformCount);
        for (int i = 0; i < uniformCount; i++)
        {
            var key = GL.GetActiveUniform(_handle, i, out _, out _);
            var location = GL.GetUniformLocation(_handle, key);

            _uniforms.Add(key, location);
        }
        GL.UseProgram(_handle);
        GLUtil.CheckError();
    }

    public void Use()
    {
        GL.UseProgram(_handle);
    }

    private static void CompileShader(int shader)
    {
        GL.CompileShader(shader);

        GL.GetShader(shader, ShaderParameter.CompileStatus, out var status);
        if (status is not (int)All.True)
        {
            var infoLog = GL.GetShaderInfoLog(shader);
            GL.GetShader(shader, ShaderParameter.ShaderType, out var type);
            throw new Exception($"Error while compiling shader of type {type}.\n{infoLog}");
        }
    }

    private static void LinkProgram(int program)
    {
        GL.LinkProgram(program);
        GLUtil.CheckError();

        GL.GetProgram(program, GetProgramParameterName.LinkStatus, out var status);
        if (status is not (int)All.True)
        {
            throw new Exception($"Error occured while linking program {program}.");
        }
    }

    /// <summary>
    /// Set a uniform int on this shader.
    /// </summary>
    /// <param name="name">The name of the uniform</param>
    /// <param name="data">The data to set</param>
    public void SetUniform(string name, int data)
    {
        GL.UseProgram(_handle);
        GL.Uniform1(_uniforms[name], data);
        GLUtil.CheckError();
    }

    /// <summary>
    /// Set a uniform float on this shader.
    /// </summary>
    /// <param name="name">The name of the uniform</param>
    /// <param name="data">The data to set</param>
    public void SetUniform(string name, float data)
    {
        GL.UseProgram(_handle);
        GL.Uniform1(_uniforms[name], data);
        GLUtil.CheckError();
    }

    /// <summary>
    /// Set a uniform Matrix4 on this shader
    /// </summary>
    /// <param name="name">The name of the uniform</param>
    /// <param name="data">The data to set</param>
    /// <remarks>
    ///   <para>
    ///   The matrix is transposed before being sent to the shader.
    ///   </para>
    /// </remarks>
    public void SetUniform(string name, Matrix4 data)
    {
        GL.UseProgram(_handle);
        GL.UniformMatrix4(_uniforms[name], true, ref data);
        GLUtil.CheckError();
    }

    /// <summary>
    /// Set a uniform Vector3 on this shader.
    /// </summary>
    /// <param name="name">The name of the uniform</param>
    /// <param name="data">The data to set</param>
    public void SetUniform(string name, Vector3 data)
    {
        GL.UseProgram(_handle);
        GL.Uniform3(_uniforms[name], data);
        GLUtil.CheckError();
    }

    public void Dispose()
    {
        GL.DeleteProgram(_handle);
    }
}
