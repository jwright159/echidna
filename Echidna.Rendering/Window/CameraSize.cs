using Echidna.Core;
using Echidna.Rendering.Shader;
using OpenTK.Mathematics;

namespace Echidna.Rendering.Window;

public class CameraSize : EntityComponent
{
	public readonly Window Window;
	public Vector2i Size;
	
	public CameraSize(Window window, Vector2i size)
	{
		Window = window;
		Size = size;
	}
}