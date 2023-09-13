using Echidna.Core;

namespace Echidna.Rendering.Shader;

public class Shader : Component
{
	internal int Handle;
	
	internal readonly string VertexSource;
	internal readonly string FragmentSource;
	
	internal readonly Dictionary<string, int> Uniforms = new();
	
	internal bool HasBeenDisposed;
	
	public Shader(string vertexSource, string fragmentSource)
	{
		VertexSource = vertexSource;
		FragmentSource = fragmentSource;
	}
	
	~Shader()
	{
		if (!HasBeenDisposed)
			Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
	}
}