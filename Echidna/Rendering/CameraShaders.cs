namespace Echidna.Rendering;

public class CameraShaders : Component
{
	internal readonly Shader[] shaders;
	
	public CameraShaders(params Shader[] shaders)
	{
		this.shaders = shaders;
	}
}