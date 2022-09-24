using ArcticFoxEngine.Math;
using Silk.NET.OpenAL;
using Silk.NET.OpenGL;

namespace ArcticFoxEngine.Rendering.OpenGL;

internal class Shader : IDisposable
{
    private readonly GL _gl;

    private readonly uint _handle;
    private readonly Dictionary<string, int> _uniforms;

    public Shader(GL gl, string vert, string frag)
    {
        _gl = gl;

        var vertexShader = _gl.CreateShader(ShaderType.VertexShader);
        _gl.ShaderSource(vertexShader, vert);
        CompileShader(vertexShader);

        var fragmentShader = _gl.CreateShader(ShaderType.FragmentShader);
        _gl.ShaderSource(fragmentShader, frag);
        CompileShader(fragmentShader);

        _handle = _gl.CreateProgram();
        _gl.CheckError();

        _gl.AttachShader(_handle, vertexShader);
        _gl.AttachShader(_handle, fragmentShader);

        LinkProgram(_handle);
        _gl.GetProgram(_handle, ProgramPropertyARB.LinkStatus, out var linkStatus);

        _gl.DetachShader(_handle, vertexShader);
        _gl.DetachShader(_handle, fragmentShader);
        _gl.DeleteShader(vertexShader);
        _gl.DeleteShader(fragmentShader);

        _uniforms = new Dictionary<string, int>();
        _gl.GetProgram(_handle, ProgramPropertyARB.ActiveUniforms, out var uniformCount);
        for (uint i = 0; i < uniformCount; i++)
        {
            var key = _gl.GetActiveUniform(_handle, i, out _, out _);
            var location = _gl.GetUniformLocation(_handle, key);

            _uniforms.Add(key, location);
        }
        _gl.UseProgram(_handle);
        _gl.CheckError();
    }

    public void Use()
    {
        _gl.UseProgram(_handle);
    }

    private void CompileShader(uint shader)
    {
        _gl.CompileShader(shader);

        _gl.GetShader(shader, ShaderParameterName.CompileStatus, out var status);
        if (status is not (int)GLEnum.True)
        {
            var infoLog = _gl.GetShaderInfoLog(shader);
            _gl.GetShader(shader, ShaderParameterName.ShaderType, out var type);
            throw new Exception($"Error while compiling shader of type {type}.\n{infoLog}");
        }
    }

    private void LinkProgram(uint program)
    {
        _gl.LinkProgram(program);
        _gl.CheckError();

        _gl.GetProgram(program, ProgramPropertyARB.LinkStatus, out var status);
        if (status is not (int)GLEnum.True)
        {
            throw new Exception($"Error occured while linking program {program}.");
        }
    }

    [Obsolete]
    public unsafe void SetUniform(string name, Silk.NET.Maths.Matrix4X4<float> mat)
    {
        _gl.UseProgram(_handle);
        _gl.UniformMatrix4(_uniforms[name], 1, true, (float*)&mat);
        _gl.CheckError();
    }

    /// <summary>
    /// Set a uniform int on this shader.
    /// </summary>
    /// <param name="name">The name of the uniform</param>
    /// <param name="data">The data to set</param>
    public void SetUniform(string name, int data)
    {
        _gl.UseProgram(_handle);
        _gl.Uniform1(_uniforms[name], data);
        _gl.CheckError();
    }

    /// <summary>
    /// Set a uniform float on this shader.
    /// </summary>
    /// <param name="name">The name of the uniform</param>
    /// <param name="data">The data to set</param>
    public void SetUniform(string name, float data)
    {
        _gl.UseProgram(_handle);
        _gl.Uniform1(_uniforms[name], data);
        _gl.CheckError();
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
    public unsafe void SetUniform(string name, Matrix4x4 data)
    {
        _gl.UseProgram(_handle);
        _gl.UniformMatrix4(_uniforms[name], 1, true, (float*)&data);
        _gl.CheckError();
    }

    /// <summary>
    /// Set a uniform Vector3 on this shader.
    /// </summary>
    /// <param name="name">The name of the uniform</param>
    /// <param name="data">The data to set</param>
    public unsafe void SetUniform(string name, Vector3 data)
    {
        _gl.UseProgram(_handle);
        _gl.Uniform3(_uniforms[name], 1, (float*)&data);
        _gl.CheckError();
    }

    public void Dispose()
    {
        _gl.DeleteProgram(_handle);
    }
}
