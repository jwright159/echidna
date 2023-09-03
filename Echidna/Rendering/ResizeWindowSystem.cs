using Echidna.Core;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Echidna.Rendering;

public class ResizeWindowSystem : System<CameraResizer>
{
	protected override void OnDrawEach(CameraResizer resizer)
	{
		Vector2i size = resizer.Window.GameWindow.Size;
		if (size == resizer.Size) return;
		
		resizer.Size = size;
		GL.Viewport(0, 0, size.X, size.Y);
		resizer.Projection.AspectRatio = (float)size.X / size.Y;
	}
}