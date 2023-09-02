using OpenTK.Mathematics;

namespace Echidna.Hierarchy;

public class SpinnerSystem : System<Transform, Spinner>
{
	protected override void OnUpdate(float deltaTime, IEnumerable<(Transform, Spinner)> componentSets)
	{
		componentSets.AsParallel().ForAll(tuple =>
		{
			tuple.Item2.currentAngle += deltaTime * tuple.Item2.speed;
			tuple.Item1.LocalRotation = Quaternion.FromAxisAngle(tuple.Item2.axis, tuple.Item2.currentAngle);
		});
	}
}