using BepuPhysics;
using Echidna.Core;
using Echidna.Hierarchy;

namespace Echidna.Physics;

public class StaticBodyTransformSystem : System<Transform, SimulationTarget, BodyShape, StaticBody>
{
	protected override void OnInitializeEach(Transform transform, SimulationTarget target, BodyShape shape, StaticBody body)
	{
		body.Handle = target.Simulation.Statics.Add(new StaticDescription(transform.LocalPosition, shape.AddToShapes(target.Simulation.Shapes)));
	}
}