using System.Diagnostics;

namespace Echidna.Hierarchy;

public class Lifetime : Component
{
	internal readonly Stopwatch time = new();
}