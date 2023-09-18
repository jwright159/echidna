using Echidna.Core;
using OpenTK.Mathematics;

namespace Echidna.Rendering.Shader;

public class Shader2dSystem : System<Shader, Scene2dObject, CurrentScene>
{
	private static Matrix4 viewMatrix = Matrix4.CreateTranslation(Mathematics.Vector3.Out).Inverted();
	
	protected override void OnDraw(IEnumerable<(Shader, Scene2dObject, CurrentScene)> componentSets)
	{
		Scene2d? currentScene = null;
		Matrix4 projectionMatrix = Matrix4.Identity;
		
		foreach ((Shader shader, Scene2dObject _, CurrentScene scene) in componentSets)
		{
			if (currentScene != scene.Scene2d)
			{
				currentScene = scene.Scene2d;
				projectionMatrix = Matrix4.CreateOrthographic(scene.Scene2d.CameraSize.Size.X, scene.Scene2d.CameraSize.Size.Y, 1.0f, 101);
			}
			
			shader.Bind();
			shader.SetMatrix4("view", viewMatrix);
			shader.SetMatrix4("projection", projectionMatrix);
		}
	}
}