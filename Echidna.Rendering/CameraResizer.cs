using Echidna.Core;
using OpenTK.Mathematics;

namespace Echidna.Rendering;

public class CameraResizer : Component
{
	public readonly Window Window;
	public readonly Perspective Perspective;
	public Vector2i Size;
	
	public CameraResizer(Window window, Perspective perspective, Vector2i size)
	{
		Window = window;
		Perspective = perspective;
		Size = size;
	}
}