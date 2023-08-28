namespace Echidna;

public class Entity
{
	private Guid id = Guid.NewGuid();
	
	private Dictionary<Type, Component> components = new();
	
	public T GetComponent<T>() where T : Component => (T)components[typeof(T)];
	
	internal void AddComponent(Component component) => components.Add(component.GetType(), component);
	
	public bool HasComponentType(Type componentType) => components.ContainsKey(componentType);
}