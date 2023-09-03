using BepuPhysics;

namespace Echidna.Physics;

public class DynamicBody : Component
{
	internal readonly BodyInertia Inertia;
	internal BodyHandle Handle;
	
	public DynamicBody(BodyInertia inertia)
	{
		Inertia = inertia;
	}
}