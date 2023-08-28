using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Echidna;

public class Shader : Component
{
	private int handle;
	
	private bool initialized;
	private bool disposed;
	
	private string vertexSource;
	private string fragmentSource;
	
	private Dictionary<string, int> uniforms = new();
	
	public Shader(string vertexPath, string fragmentPath)
	{
		vertexSource = File.ReadAllText(vertexPath);
		fragmentSource = File.ReadAllText(fragmentPath);
	}
	
	public void Initialize()
	{
		if (initialized) return;
		initialized = true;
		
		int vertexShader = CompileShader(vertexSource, ShaderType.VertexShader);
		int fragmentShader = CompileShader(fragmentSource, ShaderType.FragmentShader);
		
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
		
		GL.DetachShader(handle, vertexShader);
		GL.DeleteShader(vertexShader);
		GL.DetachShader(handle, fragmentShader);
		GL.DeleteShader(fragmentShader);
		
		GL.GetProgram(handle, GetProgramParameterName.ActiveUniforms, out int numberOfUniforms);
		for (int i = 0; i < numberOfUniforms; i++)
		{
			string key = GL.GetActiveUniform(handle, i, out _, out _);
			int location = GL.GetUniformLocation(handle, key);
			uniforms.Add(key, location);
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
	
	public int GetAttribLocation(string attribName)
	{
		return GL.GetAttribLocation(handle, attribName);
	}
	
	public int GetUniformLocation(string uniformName)
	{
		return uniforms[uniformName];
	}
	
	public void SetInt(string name, int data)
	{
		Bind();
		GL.Uniform1(GetUniformLocation(name), data);
	}
	
	public void SetFloat(string name, float data)
	{
		Bind();
		GL.Uniform1(GetUniformLocation(name), data);
	}
	
	public void SetMatrix4(string name, Matrix4 data)
	{
		Bind();
		GL.UniformMatrix4(GetUniformLocation(name), true, ref data);
	}
	
	public void SetVector3(string name, Vector3 data)
	{
		Bind();
		GL.Uniform3(GetUniformLocation(name), data);
	}
	
	public void Bind()
	{
		if (!initialized) throw new InvalidOperationException("Object wasn't initialized");
		
		GL.UseProgram(handle);
	}
	
	public void Dispose()
	{
		if (disposed) return;
		disposed = true;
		if (!initialized) return;
		
		GL.DeleteProgram(handle);
	}
	
	~Shader()
	{
		if (!disposed)
			Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
	}
}