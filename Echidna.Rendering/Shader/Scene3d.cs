using Echidna.Core;
using Echidna.Hierarchy;

namespace Echidna.Rendering.Shader;

public class Scene3d : Component
{
	public Transform CameraTransform;
	public Perspective CameraPerspective;
	
	public Scene3d(Transform cameraTransform, Perspective cameraPerspective)
	{
		CameraTransform = cameraTransform;
		CameraPerspective = cameraPerspective;
	}
	
	public Scene3d(Entity camera) : this(camera.GetComponent<Transform>(), camera.GetComponent<Perspective>()) { }
}