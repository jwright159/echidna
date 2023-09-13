using Echidna.Core;

namespace Echidna.Rendering.Texture;

public class Texture : Component
{
	internal int Handle;
	
	internal readonly string Path;
	
	internal bool HasBeenDisposed;
	
	public Texture(string path)
	{
		Path = path;
	}
	
	~Texture()
	{
		if (!HasBeenDisposed)
			Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
	}
}