using OpenTK.Mathematics;

namespace Echidna.Hierarchy;

public class SpinnerSystem : System
{
	public SpinnerSystem() : base(typeof(Transform), typeof(Spinner)) { }
	
	[UpdateEach]
	private static void Spin(float deltaTime, Transform transform, Spinner spinner)
	{
		spinner.currentAngle += deltaTime * spinner.speed;
		transform.LocalRotation = Quaternion.FromAxisAngle(spinner.axis, spinner.currentAngle);
	}
}