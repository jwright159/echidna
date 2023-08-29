using OpenTK.Mathematics;

namespace Echidna.Hierarchy;

public class Spinner : Component
{
	internal Vector3 axis;
	internal float speed;
	internal float currentAngle;
	
	public Spinner(Vector3 axis, float speed)
	{
		this.axis = axis;
		this.speed = speed;
	}
}