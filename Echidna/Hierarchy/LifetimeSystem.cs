namespace Echidna.Hierarchy;

public class LifetimeSystem : System
{
	public LifetimeSystem() : base(typeof(Lifetime)) { }
	
	[InitializeEach]
	private static void StartTimer(Lifetime lifetime)
	{
		lifetime.time.Start();
	}
}