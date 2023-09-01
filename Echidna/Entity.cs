using System.Diagnostics.Contracts;

namespace Echidna;

public class Entity
{
	private Guid id = Guid.NewGuid();
	
	private Dictionary<Type, Component> components = new();
	
	[Pure]
	public T GetComponent<T>() where T : Component => (T)GetComponent(typeof(T));
	[Pure]
	public Component GetComponent(Type type) => components[type];
	
	internal void AddComponent(Component component) => components.Add(component.GetType(), component);
	
	[Pure]
	public bool HasComponentType<T>() where T : Component => HasComponentType(typeof(T));
	[Pure]
	public bool HasComponentType(Type componentType) => components.ContainsKey(componentType);
}