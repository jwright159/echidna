namespace Echidna.Rendering;

public class CameraShaders : Component
{
	internal readonly Shader[] Shaders;
	
	public CameraShaders(params Shader[] shaders)
	{
		Shaders = shaders;
	}
}