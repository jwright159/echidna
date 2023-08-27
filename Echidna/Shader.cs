using OpenTK.Graphics.OpenGL4;

namespace Echidna;

public class Shader : IDisposable
{
	private readonly int handle;
	private bool disposed;
	
	public Shader(string vertexPath, string fragmentPath)
	{
		int vertexShader = CompileShader(vertexPath, ShaderType.VertexShader);
		int fragmentShader = CompileShader(fragmentPath, ShaderType.FragmentShader);
		
		handle = GL.CreateProgram();
		GL.AttachShader(handle, vertexShader);
		GL.AttachShader(handle, fragmentShader);
		GL.LinkProgram(handle);
		
		GL.GetProgram(handle, GetProgramParameterName.LinkStatus, out int success);
		if (success == 0)
		{
			string infoLog = GL.GetProgramInfoLog(handle);
			Console.WriteLine(infoLog);
		}
	}
	
	~Shader()
	{
		if (!disposed)
			Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
	}
	
	private static int CompileShader(string path, ShaderType type)
	{
		string source = File.ReadAllText(path);
		
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
	
	public int GetAttribLocation(string attribName)
	{
		return GL.GetAttribLocation(handle, attribName);
	}
	
	public int GetUniformLocation(string uniformName)
	{
		return GL.GetUniformLocation(handle, uniformName);
	}
	
	public void Bind()
	{
		GL.UseProgram(handle);
	}
	
	public void Dispose()
	{
		if (!disposed)
		{
			GL.DeleteProgram(handle);
			disposed = true;
		}
		GC.SuppressFinalize(this);
	}
}