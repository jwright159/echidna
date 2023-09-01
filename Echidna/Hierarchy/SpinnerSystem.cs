using OpenTK.Mathematics;

namespace Echidna.Hierarchy;

public class SpinnerSystem : System<Transform, Spinner>
{
	protected override void OnUpdateEach(float deltaTime, Transform transform, Spinner spinner)
	{
		spinner.currentAngle += deltaTime * spinner.speed;
		transform.LocalRotation = Quaternion.FromAxisAngle(spinner.axis, spinner.currentAngle);
	}
}