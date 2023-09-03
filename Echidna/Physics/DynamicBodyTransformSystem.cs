using BepuPhysics;
using Echidna.Hierarchy;

namespace Echidna.Physics;

public class DynamicBodyTransformSystem : System<Transform, BodyShape, DynamicBody>
{
	protected override void OnInitializeEach(Transform transform, BodyShape shape, DynamicBody body)
	{
		body.Handle = shape.Simulation.Simulation!.Bodies.Add(BodyDescription.CreateDynamic(transform.LocalPosition, body.Inertia, shape.AddToSimulation(), 0.01f));
	}
	
	protected override void OnUpdateEach(float deltaTime, Transform transform, BodyShape shape, DynamicBody body)
	{
		RigidPose pose = shape.Simulation.Simulation!.Bodies[body.Handle].Pose;
		transform.LocalPosition = pose.Position;
		transform.LocalRotation = pose.Orientation;
	}
}