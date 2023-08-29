using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Echidna.Rendering;

public class Shader : Component
{
	internal int handle;
	
	internal readonly string vertexSource;
	internal readonly string fragmentSource;
	
	internal readonly Dictionary<string, int> uniforms = new();
	
	internal bool hasBeenDisposed;
	
	public Shader(string vertexPath, string fragmentPath)
	{
		vertexSource = File.ReadAllText(vertexPath);
		fragmentSource = File.ReadAllText(fragmentPath);
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
		GL.UseProgram(handle);
	}
	
	~Shader()
	{
		if (!hasBeenDisposed)
			Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
	}
}