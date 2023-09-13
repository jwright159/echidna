using Echidna.Core;
using Echidna.Hierarchy;
using OpenTK.Mathematics;

namespace Echidna.Rendering.Shader;

public class CameraShaderPerspectiveSystem : System<Transform, Perspective, CameraShaders3d>
{
	protected override void OnDrawEach(Transform transform, Perspective perspective, CameraShaders3d cameraShaders)
	{
		Matrix4 viewMatrix = transform.Transformation.Inverted();
		Matrix4 projectionMatrix = perspective.ProjectionMatrix;
		foreach (Shader shader in cameraShaders.Shaders)
		{
			shader.Bind();
			shader.SetMatrix4("view", viewMatrix);
			shader.SetMatrix4("projection", projectionMatrix);
		}
	}
}