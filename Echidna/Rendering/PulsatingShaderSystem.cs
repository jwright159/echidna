using Echidna.Hierarchy;

namespace Echidna.Rendering;

public class PulsatingShaderSystem : System
{
	public PulsatingShaderSystem() : base(typeof(PulsatingShader), typeof(Lifetime)) { }
	
	public override void OnDraw(float deltaTime)
	{
		foreach (Entity entity in Entities)
			UpdatePulse(entity.GetComponent<PulsatingShader>(), entity.GetComponent<Lifetime>());
	}
	
	private static void UpdatePulse(PulsatingShader shader, Lifetime lifetime)
	{
		shader.shader.SetVector3("someColor", (0f, MathF.Sin((float)lifetime.time.Elapsed.TotalSeconds) / 2.0f + 0.5f, 0f));
	}
}