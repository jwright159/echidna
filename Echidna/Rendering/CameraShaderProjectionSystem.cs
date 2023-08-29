using Echidna.Hierarchy;

namespace Echidna.Rendering;

public class CameraShaderProjectionSystem : System
{
	public CameraShaderProjectionSystem() : base(typeof(Transform), typeof(Projection), typeof(Shaders)) { }
	
	public override void OnDraw(float deltaTime)
	{
		foreach (Entity entity in Entities)
			SetProjectionMatrices(entity.GetComponent<Transform>(), entity.GetComponent<Projection>(), entity.GetComponent<Shaders>());
	}
	
	private static void SetProjectionMatrices(Transform transform, Projection projection, Shaders shaders)
	{
		foreach (Shader shader in shaders.shaders)
		{
			shader.SetMatrix4("view", transform.Transformation.Inverted());
			shader.SetMatrix4("projection", projection.ProjectionMatrix);
		}
	}
}