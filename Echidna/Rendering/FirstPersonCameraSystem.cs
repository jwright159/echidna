using Echidna.Hierarchy;
using OpenTK.Mathematics;

namespace Echidna.Rendering;

public class FirstPersonCameraSystem : System
{
	public FirstPersonCameraSystem() : base(typeof(Transform), typeof(FirstPersonCamera)) { }
	
	public override void OnUpdate(float deltaTime)
	{
		foreach (Entity entity in Entities)
			Update(deltaTime, entity.GetComponent<Transform>(), entity.GetComponent<FirstPersonCamera>());
	}
	
	private static void Update(float deltaTime, Transform transform, FirstPersonCamera firstPerson)
	{
		transform.LocalPosition += firstPerson.movement * firstPerson.movementSpeed * deltaTime;
		transform.LocalRotation = Quaternion.FromEulerAngles(MathHelper.DegreesToRadians(firstPerson.Pitch), MathHelper.DegreesToRadians(firstPerson.Yaw), 0);
	}
}