using OpenTK.Mathematics;

namespace Echidna.Hierarchy;

public class SpinnerSystem : System
{
	public SpinnerSystem() : base(typeof(Transform), typeof(Lifetime), typeof(Spinner)) { }
	
	[UpdateEach]
	private static void Spin(Transform transform, Lifetime lifetime, Spinner spinner)
	{
		spinner.currentAngle += lifetime.DeltaTime * spinner.speed;
		transform.LocalRotation = Quaternion.FromAxisAngle(spinner.axis, spinner.currentAngle);
	}
}