using Echidna.Hierarchy;

namespace Echidna.Rendering;

public class CameraShaderProjectionSystem : System
{
	public CameraShaderProjectionSystem() : base(typeof(Transform), typeof(Projection), typeof(CameraShaders)) { }
	
	[DrawEach]
	private static void SetProjectionMatrices(Transform transform, Projection projection, CameraShaders cameraShaders)
	{
		foreach (Shader shader in cameraShaders.shaders)
		{
			shader.Bind();
			shader.SetMatrix4("view", transform.Transformation.Inverted());
			shader.SetMatrix4("projection", projection.ProjectionMatrix);
		}
	}
}