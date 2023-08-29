using Echidna.Hierarchy;

namespace Echidna.Rendering;

public class FirstPersonCameraSystem : System
{
	public FirstPersonCameraSystem() : base(typeof(Transform), typeof(FirstPersonCamera)) { }
	
	
}