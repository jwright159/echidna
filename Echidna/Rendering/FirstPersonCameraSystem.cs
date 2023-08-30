using Echidna.Hierarchy;

namespace Echidna.Rendering;

public class FirstPersonCameraSystem : System
{
	public FirstPersonCameraSystem() : base(typeof(Transform), typeof(Lifetime), typeof(FirstPersonCamera)) { }
	
	[UpdateEach]
	private static void Update(Transform transform, Lifetime lifetime, FirstPersonCamera firstPerson)
	{
		transform.LocalPosition += transform.TransformDirection(firstPerson.movement) * firstPerson.movementSpeed * lifetime.DeltaTime;
		transform.LocalRotation = firstPerson.Rotation;
	}
}