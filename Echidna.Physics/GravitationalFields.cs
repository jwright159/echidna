using Echidna.Core;

namespace Echidna.Physics;

public class GravitationalFields : WorldComponent
{
	internal List<GravitationalField> Gravities = new();
}