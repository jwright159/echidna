using Echidna.Core;

namespace Echidna.Physics;

public class GravitationalFields : Component, WorldComponent
{
	internal List<GravitationalField> Gravities = new();
}