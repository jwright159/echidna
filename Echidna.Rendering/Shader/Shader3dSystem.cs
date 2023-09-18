using Echidna.Core;
using OpenTK.Mathematics;

namespace Echidna.Rendering.Shader;

public class Shader3dSystem : System<Shader, Scene3dObject>
{
	private static Matrix4 flip = new Matrix4(
		1, 0, 0, 0,
		0, 0, 1, 0,
		0, -1, 0, 0,
		0, 0, 0, 1
	).Inverted();
	
	protected override void OnDraw(IEnumerable<(Shader, Scene3dObject)> componentSets)
	{
		Scene3d? currentScene = null;
		Matrix4 viewMatrix = Matrix4.Identity;
		Matrix4 projectionMatrix = Matrix4.Identity;
		
		foreach ((Shader shader, Scene3dObject scene) in componentSets)
		{
			if (currentScene != scene.Scene)
			{
				currentScene = scene.Scene;
				viewMatrix = scene.Scene.CameraTransform.Transformation.Inverted() * flip;
				projectionMatrix = scene.Scene.CameraPerspective.ProjectionMatrix;
			}
			
			shader.Bind();
			shader.SetMatrix4("view", viewMatrix);
			shader.SetMatrix4("projection", projectionMatrix);
		}
	}
}