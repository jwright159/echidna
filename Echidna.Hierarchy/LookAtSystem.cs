using Echidna.Core;
using Echidna.Mathematics;

namespace Echidna.Hierarchy;

public class LookAtSystem : System<Transform, LookAt>
{
	protected override void OnDrawEach(Transform transform, LookAt lookAt)
	{
		transform.LocalRotation = Quaternion.LookAt(transform.LocalPosition, lookAt.Target.LocalPosition, Vector3.Up);
	}
}