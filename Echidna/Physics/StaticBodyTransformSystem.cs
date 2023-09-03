using BepuPhysics;
using Echidna.Hierarchy;

namespace Echidna.Physics;

public class StaticBodyTransformSystem : System<Transform, BodyShape, StaticBody>
{
	protected override void OnInitializeEach(Transform transform, BodyShape shape, StaticBody body)
	{
		body.Handle = shape.Simulation.Simulation!.Statics.Add(new StaticDescription(transform.LocalPosition, shape.AddToSimulation()));
	}
}