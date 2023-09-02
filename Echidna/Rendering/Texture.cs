namespace Echidna.Rendering;

public class Texture : Component
{
	internal int handle;
	
	internal readonly string path;
	
	internal bool hasBeenDisposed;
	
	public Texture(string path)
	{
		this.path = path;
	}
	
	~Texture()
	{
		if (!hasBeenDisposed)
			Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
	}
}