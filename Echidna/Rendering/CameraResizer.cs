using OpenTK.Mathematics;

namespace Echidna.Rendering;

public class CameraResizer : Component
{
	public readonly Window Window;
	public readonly Projection Projection;
	public Vector2i Size;
	
	public CameraResizer(Window window, Projection projection, Vector2i size)
	{
		Window = window;
		Projection = projection;
		Size = size;
	}
}