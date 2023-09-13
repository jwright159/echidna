using Echidna.Core;

namespace Echidna.Rendering.Texture;

public class CubeMap : Component
{
	internal int Handle;
	
	internal readonly string RightPath;
	internal readonly string LeftPath;
	internal readonly string ForwardPath;
	internal readonly string BackPath;
	internal readonly string UpPath;
	internal readonly string DownPath;
	
	internal bool HasBeenDisposed;
	
	public CubeMap(string rightPath, string leftPath, string forwardPath, string backPath, string upPath, string downPath)
	{
		RightPath = rightPath;
		LeftPath = leftPath;
		ForwardPath = forwardPath;
		BackPath = backPath;
		UpPath = upPath;
		DownPath = downPath;
	}
	
	~CubeMap()
	{
		if (!HasBeenDisposed)
			Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
	}
}