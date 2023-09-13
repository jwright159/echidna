using Echidna.Core;
using Echidna.Rendering.Window;
using OpenTK.Mathematics;

namespace Echidna.Rendering.Shader;

public class CameraShader2dSystem : System<CameraResizer, CameraShaders2d>
{
	private static Matrix4 viewMatrix = Matrix4.CreateTranslation(Mathematics.Vector3.Out).Inverted();
	
	protected override void OnDrawEach(CameraResizer resizer, CameraShaders2d cameraShaders)
	{
		Matrix4 projectionMatrix = Matrix4.CreateOrthographic(resizer.Size.X, resizer.Size.Y, 1.0f, 101);
		foreach (Shader shader in cameraShaders.Shaders)
		{
			shader.Bind();
			shader.SetMatrix4("view", viewMatrix);
			shader.SetMatrix4("projection", projectionMatrix);
		}
	}
}