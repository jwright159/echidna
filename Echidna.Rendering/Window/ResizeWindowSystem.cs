using Echidna.Core;
using Echidna.Rendering.Shader;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Echidna.Rendering.Window;

public class ResizeWindowSystem : System<CameraSize, Perspective>
{
	protected override void OnDrawEach(CameraSize size, Perspective perspective)
	{
		Vector2i newSize = size.Window.GameWindow.Size;
		if (newSize == size.Size) return;
		
		size.Size = newSize;
		GL.Viewport(0, 0, newSize.X, newSize.Y);
		perspective.AspectRatio = (float)newSize.X / newSize.Y;
	}
}