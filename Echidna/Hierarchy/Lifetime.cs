using System.Diagnostics;

namespace Echidna.Hierarchy;

public class Lifetime : Component
{
	internal readonly Stopwatch watch = new();
	internal float previousTime;
	public float Time => (float)watch.Elapsed.TotalSeconds;
	public float DeltaTime { get; internal set; }
}