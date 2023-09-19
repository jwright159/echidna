using Echidna.Core;
using Echidna.Rendering.Window;

namespace Echidna.Rendering.Shader;

public class Scene2d : InstanceComponent
{
	public CameraSize CameraSize;
	
	public Scene2d(CameraSize cameraSize)
	{
		CameraSize = cameraSize;
	}
	
	public Scene2d(Entity camera) : this(camera.GetComponent<CameraSize>()) { }
}