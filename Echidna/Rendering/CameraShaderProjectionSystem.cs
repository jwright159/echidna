using Echidna.Hierarchy;

namespace Echidna.Rendering;

public class CameraShaderProjectionSystem : System
{
	public CameraShaderProjectionSystem() : base(typeof(Transform), typeof(Projection), typeof(Shaders)) { }
	
	[DrawEach]
	private static void SetProjectionMatrices(Transform transform, Projection projection, Shaders shaders)
	{
		foreach (Shader shader in shaders.shaders)
		{
			shader.SetMatrix4("view", transform.Transformation.Inverted());
			shader.SetMatrix4("projection", projection.ProjectionMatrix);
		}
	}
}