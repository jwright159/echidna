using Echidna.Core;
using Echidna.Hierarchy;
using Echidna.Mathematics;

namespace Echidna.Physics;

public class GravitationalField : Component
{
	public float GravityConstant;
	internal Transform CenterTransform;
	public Vector3 Center => CenterTransform.LocalPosition;
	
	public GravitationalField(float gravityConstant, Transform center, GravitationalFields fields)
	{
		GravityConstant = gravityConstant;
		CenterTransform = center;
		fields.Gravities.Add(this);
	}
}