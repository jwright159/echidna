using Echidna.Core;

namespace Echidna.Rendering.Shader;

public class CurrentScene : Component, WorldComponent
{
	public Scene2d Scene2d;
	public Scene3d Scene3d;
	
	public CurrentScene(Scene2d scene2d, Scene3d scene3d)
	{
		Scene2d = scene2d;
		Scene3d = scene3d;
	}
}