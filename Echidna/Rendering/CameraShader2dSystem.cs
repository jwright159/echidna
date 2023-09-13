using Echidna.Core;
using Echidna.Hierarchy;
using OpenTK.Mathematics;

namespace Echidna.Rendering;

public class CameraShader2dSystem : System<CameraResizer, CameraShaders2d>
{
	protected override void OnDrawEach(CameraResizer resizer, CameraShaders2d cameraShaders)
	{
		Matrix4 viewMatrix = Matrix4.CreateTranslation(0, -1, 0).Inverted();
		Matrix4 projectionMatrix = Matrix4.CreateOrthographic(resizer.Size.X, resizer.Size.Y, 0.5f, 1000);
		foreach (Shader shader in cameraShaders.Shaders)
		{
			shader.Bind();
			shader.SetMatrix4("view", viewMatrix);
			shader.SetMatrix4("projection", projectionMatrix);
		}
	}
}