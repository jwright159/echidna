using Echidna.Core;
using Echidna.Hierarchy;
using Echidna.Mathematics;

namespace Echidna.Physics;

public class GravitySystem : System<Transform, SimulationTarget, DynamicBody, AffectedByGravity>
{
	protected override void OnPhysicsUpdateEach(float deltaTime, Transform transform, SimulationTarget target, DynamicBody body, AffectedByGravity gravity)
	{
		Vector3 gravitySum = Vector3.Zero;
		foreach (GravitationalField field in gravity.Fields.Gravities.Where(field => !gravity.Blacklist.Contains(field)))
		{
			gravitySum += field.GravityConstant / Vector3.DistanceSquared(transform.LocalPosition, field.Center) * Vector3.UnitFromTo(transform.LocalPosition, field.Center);
			Console.WriteLine($"Got valid field {field} for body {body}, sum at {gravitySum}");
		}
		target.Simulation.Bodies[body.Handle].Velocity.Linear += (System.Numerics.Vector3)(gravitySum * deltaTime);
	}
}