namespace Echidna.Core;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public abstract class ReflectedComponentAttribute : Attribute
{
	public abstract Component GetComponent(Entity entity, Type type);
}

public class SelfAttribute : ReflectedComponentAttribute
{
	public override Component GetComponent(Entity entity, Type type) => entity.GetComponent(type);
}