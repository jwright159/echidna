namespace Echidna;

public class LifetimeSystem : System
{
	public LifetimeSystem() : base(typeof(Lifetime)) { }
	
	public override void OnInitialize()
	{
		foreach (Entity entity in Entities)
			StartTimer(entity.GetComponent<Lifetime>());
	}
	
	public void StartTimer(Lifetime lifetime)
	{
		lifetime.time.Start();
	}
}