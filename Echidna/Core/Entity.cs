using System.Diagnostics.Contracts;

namespace Echidna.Core;

public class Entity
{
	private Guid id = Guid.NewGuid();
	
	private Dictionary<Type, Component> components = new();
	
	[Pure]
	public T GetComponent<T>() where T : Component => (T)GetComponent(typeof(T));
	[Pure]
	public Component GetComponent(Type type) => components[type];
	
	internal void AddComponent<T>(T component) where T : Component => components.Add(typeof(T), component);
	
	[Pure]
	public bool HasComponentType<T>() where T : Component => HasComponentType(typeof(T));
	[Pure]
	public bool HasComponentType(Type componentType) => components.ContainsKey(componentType);
}