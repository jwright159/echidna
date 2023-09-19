using Echidna.Core;

namespace Echidna.Physics;

public class GravitationalFields : WorldComponent
{
	internal readonly List<GravitationalField> Gravities = new();
}