namespace Echidna.Rendering;

public class Shader : Component
{
	internal int handle;
	
	internal readonly string vertexPath;
	internal readonly string fragmentPath;
	
	internal readonly Dictionary<string, int> uniforms = new();
	
	internal bool hasBeenDisposed;
	
	public Shader(string vertexPath, string fragmentPath)
	{
		this.vertexPath = vertexPath;
		this.fragmentPath = fragmentPath;
	}
	
	~Shader()
	{
		if (!hasBeenDisposed)
			Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
	}
}