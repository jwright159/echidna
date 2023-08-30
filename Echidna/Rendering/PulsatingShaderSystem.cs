using Echidna.Hierarchy;

namespace Echidna.Rendering;

public class PulsatingShaderSystem : System
{
	public PulsatingShaderSystem() : base(typeof(PulsatingShader), typeof(Lifetime)) { }
	
	[UpdateEach]
	private static void Pulse(float deltaTime, PulsatingShader shader, Lifetime lifetime)
	{
		shader.shader.SetVector3("someColor", (0f, MathF.Sin((float)lifetime.time.Elapsed.TotalSeconds) / 2.0f + 0.5f, 0f));
	}
}