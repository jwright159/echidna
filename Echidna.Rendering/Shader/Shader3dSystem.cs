using Echidna.Core;
using OpenTK.Mathematics;

namespace Echidna.Rendering.Shader;

public class Shader3dSystem : System<Shader, Scene3dObject, CurrentScene>
{
	private static Matrix4 flip = new Matrix4(
		1, 0, 0, 0,
		0, 0, 1, 0,
		0, -1, 0, 0,
		0, 0, 0, 1
	).Inverted();
	
	protected override void OnDraw(IEnumerable<(Shader, Scene3dObject, CurrentScene)> componentSets)
	{
		Scene3d? currentScene = null;
		Matrix4 viewMatrix = Matrix4.Identity;
		Matrix4 projectionMatrix = Matrix4.Identity;
		
		foreach ((Shader shader, Scene3dObject _, CurrentScene scene) in componentSets)
		{
			if (currentScene != scene.Scene3d)
			{
				currentScene = scene.Scene3d;
				viewMatrix = scene.Scene3d.CameraTransform.Transformation.Inverted() * flip;
				projectionMatrix = scene.Scene3d.CameraPerspective.ProjectionMatrix;
			}
			
			shader.Bind();
			shader.SetMatrix4("view", viewMatrix);
			shader.SetMatrix4("projection", projectionMatrix);
		}
	}
}