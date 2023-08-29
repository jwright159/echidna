using OpenTK.Mathematics;

namespace Echidna.Rendering;

public class CameraResizer : Component
{
	public readonly Window window;
	public readonly Projection projection;
	public Vector2i size;
	
	public CameraResizer(Window window, Projection projection, Vector2i size)
	{
		this.window = window;
		this.projection = projection;
		this.size = size;
	}
}