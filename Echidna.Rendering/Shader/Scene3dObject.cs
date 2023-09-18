using Echidna.Core;

namespace Echidna.Rendering.Shader;

public class Scene3dObject : Component
{
	public Scene3d Scene;
	
	public Scene3dObject(Scene3d scene)
	{
		Scene = scene;
	}
}