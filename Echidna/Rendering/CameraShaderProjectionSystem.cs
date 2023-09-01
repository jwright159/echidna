using Echidna.Hierarchy;

namespace Echidna.Rendering;

public class CameraShaderProjectionSystem : System<Transform, Projection, CameraShaders>
{
	protected override void OnDrawEach(Transform transform, Projection projection, CameraShaders cameraShaders)
	{
		foreach (Shader shader in cameraShaders.shaders)
		{
			shader.Bind();
			shader.SetMatrix4("view", transform.Transformation.Inverted());
			shader.SetMatrix4("projection", projection.ProjectionMatrix);
		}
	}
}