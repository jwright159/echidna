namespace Echidna.Rendering;

public class CubeMap : Component
{
	internal int handle;
	
	internal readonly string rightPath;
	internal readonly string leftPath;
	internal readonly string forwardPath;
	internal readonly string backPath;
	internal readonly string upPath;
	internal readonly string downPath;
	
	internal bool hasBeenDisposed;
	
	public CubeMap(string rightPath, string leftPath, string forwardPath, string backPath, string upPath, string downPath)
	{
		this.rightPath = rightPath;
		this.leftPath = leftPath;
		this.forwardPath = forwardPath;
		this.backPath = backPath;
		this.upPath = upPath;
		this.downPath = downPath;
	}
	
	~CubeMap()
	{
		if (!hasBeenDisposed)
			Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
	}
}