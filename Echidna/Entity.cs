namespace Echidna;

public class Entity
{
	private Guid id = Guid.NewGuid();
	
	private Dictionary<Type, Component> components = new();
	
	public T GetComponent<T>() where T : Component => (T)GetComponent(typeof(T));
	public Component GetComponent(Type type) => components[type];
	
	internal void AddComponent(Component component) => components.Add(component.GetType(), component);
	
	public bool HasComponentType<T>() where T : Component => HasComponentType(typeof(T));
	public bool HasComponentType(Type componentType) => components.ContainsKey(componentType);
}