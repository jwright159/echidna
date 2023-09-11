using Echidna.Core;

namespace Echidna.Rendering;

public class Shader : Component
{
	internal int Handle;
	
	internal readonly string VertexSource;
	internal readonly string FragmentPath;
	
	internal readonly Dictionary<string, int> Uniforms = new();
	
	internal bool HasBeenDisposed;
	
	public Shader(string vertexSource, string fragmentPath)
	{
		VertexSource = vertexSource;
		FragmentPath = fragmentPath;
	}
	
	~Shader()
	{
		if (!HasBeenDisposed)
			Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
	}
}