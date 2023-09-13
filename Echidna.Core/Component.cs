namespace Echidna.Core;

public abstract class Component
{
	private Guid id = Guid.NewGuid();
	
	internal Entity? Entity;
	
	public override string ToString() => Entity == null ? $"New {GetType().Name}" : $"{GetType().Name} (on {Entity})";
}