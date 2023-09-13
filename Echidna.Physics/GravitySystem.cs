using BepuPhysics;
using Echidna.Core;
using Echidna.Hierarchy;
using Echidna.Mathematics;

namespace Echidna.Physics;

public class GravitySystem : System<Transform, SimulationTarget, DynamicBody, AffectedByGravity>
{
	protected override void OnPhysicsUpdateEach(float deltaTime, Transform transform, SimulationTarget target, DynamicBody body, AffectedByGravity gravity)
	{
		Vector3 gravitySum = gravity.Fields.Gravities.Where(field => !gravity.Blacklist.Contains(field)).Aggregate(Vector3.Zero, (current, field) => current + GetGravityAt(field, transform.LocalPosition));
		
		BodyReference bodyReference = target.Simulation.Bodies[body.Handle];
		bodyReference.Velocity.Linear += (System.Numerics.Vector3)(gravitySum * deltaTime);
		bodyReference.Awake = true;
	}
	
	private static Vector3 GetGravityAt(GravitationalField field, Vector3 position) => field.GravityConstant / Vector3.DistanceSquared(position, field.Center) * Vector3.UnitFromTo(position, field.Center);
}