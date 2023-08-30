namespace Echidna.Hierarchy;

public class LifetimeSystem : System
{
	public LifetimeSystem() : base(typeof(Lifetime)) { }
	
	[InitializeEach]
	private static void StartTimer(Lifetime lifetime)
	{
		lifetime.watch.Start();
	}
	
	[UpdateEach]
	private static void UpdateDelta(Lifetime lifetime)
	{
		lifetime.DeltaTime = lifetime.Time - lifetime.previousTime;
		lifetime.previousTime = lifetime.Time;
	}
}