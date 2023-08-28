namespace Echidna;

public class CameraShaderProjectionSystem : System
{
	public CameraShaderProjectionSystem() : base(typeof(Transform), typeof(Projection), typeof(Shader)) { }
	
	public override void OnDraw()
	{
		foreach (Entity entity in Entities)
			SetProjectionMatrices(entity.GetComponent<Transform>(), entity.GetComponent<Projection>(), entity.GetComponent<Shader>());
	}
	
	private void SetProjectionMatrices(Transform transform, Projection projection, Shader shader)
	{
		shader.SetMatrix4("view", transform.Transformation);
		shader.SetMatrix4("projection", projection.ProjectionMatrix);
	}
}