using Echidna.Core;
using StbSharp.MonoGame.Test;

namespace Echidna.Rendering;

public class Font : Component
{
	internal const int TextureSize = 1024;
	
	internal int TextureHandle;
	internal int VertexBufferObject;
	internal int VertexArrayObject;
	internal readonly float[] Vertices =
	{
		0.0f, 0.0f, 1.0f,   0.0f, 0.0f,   0.0f, 0.0f, 0.0f,
		0.0f, 0.0f, 0.0f,   0.0f, 1.0f,   1.0f, 0.0f, 0.0f,
		1.0f, 0.0f, 0.0f,   1.0f, 1.0f,   0.0f, 0.0f, 1.0f,
		
		0.0f, 0.0f, 1.0f,   0.0f, 0.0f,   0.0f, 0.0f, 0.0f,
		1.0f, 0.0f, 0.0f,   1.0f, 1.0f,   0.0f, 0.0f, 1.0f,
		1.0f, 0.0f, 1.0f,   1.0f, 0.0f,   0.0f, 1.0f, 0.0f,
	};
	
	internal FontBakerResult? FontResult;
	
	internal readonly string Path;
	
	internal bool HasBeenDisposed;
	
	public Font(string path)
	{
		Path = path;
	}
	
	~Font()
	{
		if (!HasBeenDisposed)
			Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
	}
}