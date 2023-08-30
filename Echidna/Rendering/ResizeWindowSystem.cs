using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Echidna.Rendering;

public class ResizeWindowSystem : System
{
	public ResizeWindowSystem() : base(typeof(CameraResizer)) { }
	
	[DrawEach]
	private static void Resize(float deltaTime, CameraResizer resizer)
	{
		Vector2i size = resizer.window.window.Size;
		if (size == resizer.size) return;
		
		resizer.size = size;
		GL.Viewport(0, 0, size.X, size.Y);
		resizer.projection.AspectRatio = (float)size.X / size.Y;
	}
}