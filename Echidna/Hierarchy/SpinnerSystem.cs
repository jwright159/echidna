using Echidna.Mathematics;

namespace Echidna.Hierarchy;

public class SpinnerSystem : System<Transform, Spinner>
{
	protected override void OnUpdateEach(float deltaTime, Transform transform, Spinner spinner)
	{
		spinner.CurrentAngle += deltaTime * spinner.Speed;
		transform.LocalRotation = Quaternion.FromAxisAngle(spinner.Axis, spinner.CurrentAngle);
	}
}