using Echidna.Core;
using Echidna.Hierarchy;

namespace Echidna.Editor;

public class FirstPersonCameraSystem : System<Transform, FirstPersonCamera>
{
	protected override void OnUpdateEach(float deltaTime, Transform transform, FirstPersonCamera firstPerson)
	{
		transform.LocalPosition += transform.TransformDirection(firstPerson.Movement) * firstPerson.MovementSpeed * deltaTime;
		transform.LocalRotation = firstPerson.Rotation;
	}
}