using Echidna.Hierarchy;

namespace Echidna.Rendering;

public class PulsatingShaderSystem : System
{
	public PulsatingShaderSystem() : base(typeof(Shader), typeof(PulsatingShader), typeof(Lifetime)) { }
	
	public override void OnDraw()
	{
		foreach (Entity entity in Entities)
			UpdatePulse(entity.GetComponent<Shader>(), entity.GetComponent<Lifetime>());
	}
	
	private static void UpdatePulse(Shader shader, Lifetime lifetime)
	{
		shader.SetVector3("someColor", (0f, MathF.Sin((float)lifetime.time.Elapsed.TotalSeconds) / 2.0f + 0.5f, 0f));
	}
}