namespace Echidna.Hierarchy;

public class LifetimeSystem : System<Lifetime>
{
	protected override void OnUpdateEach(float deltaTime, Lifetime lifetime)
	{
		lifetime.Time += deltaTime;
	}
}