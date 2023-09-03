using Echidna.Core;
using Echidna.Hierarchy;

namespace Echidna.Rendering;

public class FirstPersonCameraSystem : System<Transform, Lifetime, FirstPersonCamera>
{
	protected override void OnUpdateEach(float deltaTime, Transform transform, Lifetime lifetime, FirstPersonCamera firstPerson)
	{
		transform.LocalPosition += transform.TransformDirection(firstPerson.Movement) * firstPerson.MovementSpeed * deltaTime;
		transform.LocalRotation = firstPerson.Rotation;
	}
}