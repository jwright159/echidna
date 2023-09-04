using System.Diagnostics.Contracts;

namespace Echidna.Core;

public class Entity
{
	private Guid id = Guid.NewGuid();
	public string Name;
	
	public Entity(string name)
	{
		Name = name;
	}
	public Entity() : this("New Entity") { }
	
	private Dictionary<Type, Component> components = new();
	
	[Pure]
	public T GetComponent<T>() where T : Component => (T)GetComponent(typeof(T));
	[Pure]
	public Component GetComponent(Type type) => components[type];
	
	internal void AddComponent<T>(T component) where T : Component
	{
		components.Add(typeof(T), component);
		component.Entity = this;
		Console.WriteLine($"{component} {component.Entity}");
	}
	
	[Pure]
	public bool HasComponentType<T>() where T : Component => HasComponentType(typeof(T));
	[Pure]
	public bool HasComponentType(Type componentType) => components.ContainsKey(componentType);
	
	public override string ToString() => Name;
}