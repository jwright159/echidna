using BepuPhysics;
using Echidna.Core;
using Echidna.Hierarchy;
using Echidna.Mathematics;

namespace Echidna.Physics;

public class GravitySystem : System<Transform, SimulationTarget, DynamicBody, AffectedByGravity>
{
	protected override void OnPhysicsUpdateEach(float deltaTime, Transform transform, SimulationTarget target, DynamicBody body, AffectedByGravity gravity)
	{
		Vector3 gravitySum = gravity.Fields.Gravities.Where(field => !gravity.Blacklist.Contains(field)).Aggregate(Vector3.Zero, (current, field) => current + field.GravityConstant / Vector3.DistanceSquared(transform.LocalPosition, field.Center) * Vector3.UnitFromTo(transform.LocalPosition, field.Center));
		
		BodyReference bodyReference = target.Simulation.Bodies[body.Handle];
		bodyReference.Velocity.Linear += (System.Numerics.Vector3)(gravitySum * deltaTime);
		bodyReference.Awake = true;
	}
}