using OpenTK.Graphics.OpenGL4;

namespace Echidna.Rendering;

public class ShaderSystem : System
{
	public ShaderSystem() : base(typeof(Shader)) { }
	
	[InitializeEach]
	private static void Initialize(Shader shader)
	{
		int vertexShader = CompileShader(shader.vertexSource, ShaderType.VertexShader);
		int fragmentShader = CompileShader(shader.fragmentSource, ShaderType.FragmentShader);
		
		int handle = shader.handle = GL.CreateProgram();
		GL.AttachShader(handle, vertexShader);
		GL.AttachShader(handle, fragmentShader);
		GL.LinkProgram(handle);
		
		GL.GetProgram(handle, GetProgramParameterName.LinkStatus, out int success);
		if (success == 0)
		{
			string infoLog = GL.GetProgramInfoLog(handle);
			Console.WriteLine(infoLog);
		}
		
		GL.DetachShader(handle, vertexShader);
		GL.DeleteShader(vertexShader);
		GL.DetachShader(handle, fragmentShader);
		GL.DeleteShader(fragmentShader);
		
		GL.GetProgram(handle, GetProgramParameterName.ActiveUniforms, out int numberOfUniforms);
		for (int i = 0; i < numberOfUniforms; i++)
		{
			string key = GL.GetActiveUniform(handle, i, out _, out _);
			int location = GL.GetUniformLocation(handle, key);
			shader.uniforms.Add(key, location);
		}
	}
	
	private static int CompileShader(string source, ShaderType type)
	{
		int shader = GL.CreateShader(type);
		GL.ShaderSource(shader, source);
		GL.CompileShader(shader);
		
		GL.GetShader(shader, ShaderParameter.CompileStatus, out int success);
		if (success == 0)
		{
			string infoLog = GL.GetShaderInfoLog(shader);
			Console.WriteLine(infoLog);
		}
		
		return shader;
	}
	
	[DisposeEach]
	private static void Dispose(Shader shader)
	{
		shader.hasBeenDisposed = true;
		GL.DeleteProgram(shader.handle);
	}
}