using Echidna.Core;
using Echidna.Hierarchy;
using Echidna.Mathematics;

namespace Echidna.Physics;

public class GravitationalField : EntityComponent
{
	public float GravityConstant;
	[Self] public Transform CenterTransform;
	public Vector3 Center => CenterTransform.LocalPosition;
	
	public GravitationalField(float gravityConstant, GravitationalFields fields)
	{
		GravityConstant = gravityConstant;
		fields.Gravities.Add(this);
	}
}