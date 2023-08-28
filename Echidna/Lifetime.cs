using System.Diagnostics;

namespace Echidna;

public class Lifetime : Component
{
	internal readonly Stopwatch time = new();
}