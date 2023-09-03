using BepuPhysics;
using Echidna.Core;
using Echidna.Hierarchy;

namespace Echidna.Physics;

public class DynamicBodyTransformSystem : System<Transform, SimulationTarget, BodyShape, DynamicBody>
{
	protected override void OnInitializeEach(Transform transform, SimulationTarget target, BodyShape shape, DynamicBody body)
	{
		body.Handle = target.Simulation.Bodies.Add(BodyDescription.CreateDynamic(transform.LocalPosition, body.Inertia, shape.AddToShapes(target.Simulation.Shapes), 0.01f));
	}
	
	protected override void OnUpdateEach(float deltaTime, Transform transform, SimulationTarget target, BodyShape shape, DynamicBody body)
	{
		RigidPose pose = target.Simulation.Bodies[body.Handle].Pose;
		transform.LocalPosition = pose.Position;
		transform.LocalRotation = pose.Orientation;
	}
}