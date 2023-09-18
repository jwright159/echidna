using Echidna.Core;
using OpenTK.Mathematics;

namespace Echidna.Rendering.Shader;

public class Shader2dSystem : System<Shader, Scene2dObject>
{
	private static Matrix4 viewMatrix = Matrix4.CreateTranslation(Mathematics.Vector3.Out).Inverted();
	
	protected override void OnDraw(IEnumerable<(Shader, Scene2dObject)> componentSets)
	{
		Scene2d? currentScene = null;
		Matrix4 projectionMatrix = Matrix4.Identity;
		
		foreach ((Shader shader, Scene2dObject scene) in componentSets)
		{
			if (currentScene != scene.Scene)
			{
				currentScene = scene.Scene;
				projectionMatrix = Matrix4.CreateOrthographic(scene.CameraSize.X, scene.CameraSize.Y, 1.0f, 101);
			}
			
			shader.Bind();
			shader.SetMatrix4("view", viewMatrix);
			shader.SetMatrix4("projection", projectionMatrix);
		}
	}
}