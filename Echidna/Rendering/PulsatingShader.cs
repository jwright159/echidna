namespace Echidna.Rendering;

public class PulsatingShader : Component
{
	public readonly Shader shader;
	
	public PulsatingShader(Shader shader)
	{
		this.shader = shader;
	}
}