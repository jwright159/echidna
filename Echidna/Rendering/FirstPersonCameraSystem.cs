using Echidna.Hierarchy;

namespace Echidna.Rendering;

public class FirstPersonCameraSystem : System<Transform, Lifetime, FirstPersonCamera>
{
	protected override void OnUpdateEach(float deltaTime, Transform transform, Lifetime lifetime, FirstPersonCamera firstPerson)
	{
		transform.LocalPosition += transform.TransformDirection(firstPerson.movement) * firstPerson.movementSpeed * deltaTime;
		transform.LocalRotation = firstPerson.Rotation;
	}
}