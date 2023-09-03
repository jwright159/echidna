using Echidna.Core;

namespace Echidna.Rendering;

public class PulsatingShader : Component
{
	public readonly Shader Shader;
	
	public PulsatingShader(Shader shader)
	{
		Shader = shader;
	}
}