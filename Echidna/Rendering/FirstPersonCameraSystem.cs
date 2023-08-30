using Echidna.Hierarchy;

namespace Echidna.Rendering;

public class FirstPersonCameraSystem : System
{
	public FirstPersonCameraSystem() : base(typeof(Transform), typeof(FirstPersonCamera)) { }
	
	[UpdateEach]
	private static void Update(float deltaTime, Transform transform, FirstPersonCamera firstPerson)
	{
		transform.LocalPosition += transform.TransformDirection(firstPerson.movement) * firstPerson.movementSpeed * deltaTime;
		transform.LocalRotation = firstPerson.Rotation;
	}
}