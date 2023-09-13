using Echidna.Core;

namespace Echidna.Rendering;

public class CameraShaders2d : Component
{
	internal readonly Shader[] Shaders;
	
	public CameraShaders2d(params Shader[] shaders)
	{
		Shaders = shaders;
	}
}