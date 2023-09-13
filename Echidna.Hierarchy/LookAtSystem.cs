using Echidna.Core;
using Echidna.Mathematics;

namespace Echidna.Hierarchy;

public class LookAtSystem : System<Transform, LookAt>
{
	protected override void OnDrawEach(Transform transform, LookAt lookAt)
	{
		transform.LocalRotation = Quaternion.LookAt(transform.Position, lookAt.Target.Position, Vector3.Up);
	}
}