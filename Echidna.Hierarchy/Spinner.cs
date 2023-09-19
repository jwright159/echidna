using Echidna.Core;
using Echidna.Mathematics;

namespace Echidna.Hierarchy;

public class Spinner : EntityComponent
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