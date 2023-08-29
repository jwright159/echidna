using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Echidna.Rendering;

public class ResizeWindowSystem : System
{
	public ResizeWindowSystem() : base(typeof(CameraResizer)) { }
	
	public override void OnDraw(float deltaTime)
	{
		foreach (Entity entity in Entities)
			Resize(entity.GetComponent<CameraResizer>());
	}
	
	private static void Resize(CameraResizer resizer)
	{
		Vector2i size = resizer.window.window.Size;
		if (size == resizer.size) return;
		
		resizer.size = size;
		GL.Viewport(0, 0, size.X, size.Y);
		resizer.projection.AspectRatio = (float)size.X / size.Y;
	}
}