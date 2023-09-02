using System.Numerics;
using BepuPhysics;
using BepuPhysics.Collidables;
using Echidna.Hierarchy;

namespace Echidna.Physics;

public class BodyTransformSystem : System<Transform, SimulationBody>
{
	protected override void OnInitializeEach(Transform transform, SimulationBody body)
	{
		Box shape = new(1, 1, 1);
		BodyInertia inertia = shape.ComputeInertia(1);
		body.handle = body.simulation.simulation!.Bodies.Add(BodyDescription.CreateDynamic((Vector3)transform.LocalPosition, inertia, body.simulation.simulation.Shapes.Add(shape), 0.01f));
	}
	
	protected override void OnUpdateEach(float deltaTime, Transform transform, SimulationBody body)
	{
		RigidPose pose = body.simulation.simulation!.Bodies[body.handle].Pose;
		transform.LocalPosition = pose.Position;
	}
}