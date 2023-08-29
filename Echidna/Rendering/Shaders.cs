namespace Echidna.Rendering;

public class Shaders : Component
{
	internal readonly Shader[] shaders;

	public Shaders(params Shader[] shaders)
	{
		this.shaders = shaders;
	}
}