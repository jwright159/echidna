using Echidna.Core;
using Echidna.Hierarchy;

namespace Echidna.Rendering.Shader;

public class PulsatingShader : Component
{
	public Shader Shader;
	public Lifetime Lifetime;
	
	public PulsatingShader(Shader shader, Lifetime lifetime)
	{
		Shader = shader;
		Lifetime = lifetime;
	}
}