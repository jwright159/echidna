using Echidna.Core;
using OpenTK.Mathematics;

namespace Echidna.Rendering.Shader;

public class Scene2dObject : Component
{
	public Scene2d Scene;
	
	public Vector2i CameraSize => Scene.CameraSize.Size;
	
	public Scene2dObject(Scene2d scene)
	{
		Scene = scene;
	}
}