using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Echidna;

public class World
{
	private System[] systems;
	
	private Dictionary<Type, System[]> systemsDependingOn;
	
	private Entity globalEntity = new();
	
	public World(params System[] systems)
	{
		this.systems = systems;
		
		Dictionary<Type, List<System>> systemsDepending = new();
		
		foreach (System system in systems)
		{
			foreach (Type componentType in system.ComponentTypes)
			{
				if (!systemsDepending.ContainsKey(componentType))
					systemsDepending.Add(componentType, new List<System>());
				systemsDepending[componentType].Add(system);
			}
		}
		
		systemsDependingOn = systemsDepending.ToDictionary(pair => pair.Key, pair => pair.Value.ToArray());
	}
	
	public void AddComponent(Entity entity, Component component)
	{
		if (entity.HasComponentType(component.GetType()))
			throw new InvalidOperationException($"Entity {entity} already has a component of type {component}");
		Type addedComponentType = component.GetType();
		if (!systemsDependingOn.ContainsKey(addedComponentType))
			throw new InvalidOperationException($"World {this} does not have a system depending on component type {addedComponentType}");
		
		entity.AddComponent(component);
		foreach (System system in systemsDependingOn[addedComponentType])
		{
			if (system.IsApplicableTo(entity))
				system.AddEntity(entity);
		}
	}
	
	public void AddSingletonComponent(Component component) => AddComponent(globalEntity, component);
	
	public void Initialize()
	{
		foreach (System system in systems)
			system.OnInitialize();
	}
	
	public void Dispose()
	{
		foreach (System system in systems)
			system.OnDispose();
	}
	
	public void Update()
	{
		foreach (System system in systems)
			system.OnUpdate();
	}
	
	public void Draw()
	{
		foreach (System system in systems)
			system.OnDraw();
	}
	
	public void MouseMove(Vector2 position, Vector2 delta)
	{
		foreach (System system in systems)
			system.OnMouseMove(position, delta);
	}
	
	public void KeyDown(Keys key)
	{
		foreach (System system in systems)
			system.OnKeyDown(key);
	}
	
	public void KeyUp(Keys key)
	{
		foreach (System system in systems)
			system.OnKeyUp(key);
	}
}