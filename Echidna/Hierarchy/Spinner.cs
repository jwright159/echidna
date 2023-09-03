using Echidna.Mathematics;

namespace Echidna.Hierarchy;

public class Spinner : Component
{
	internal Vector3 Axis;
	internal float Speed;
	internal float CurrentAngle;
	
	public Spinner(Vector3 axis, float speed)
	{
		Axis = axis;
		Speed = speed;
	}
}