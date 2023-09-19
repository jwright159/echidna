using BepuPhysics;
using Echidna.Core;

namespace Echidna.Physics;

public class DynamicBody : EntityComponent
{
	internal readonly BodyInertia Inertia;
	internal BodyHandle Handle;
	
	public DynamicBody(BodyInertia inertia)
	{
		Inertia = inertia;
	}
}