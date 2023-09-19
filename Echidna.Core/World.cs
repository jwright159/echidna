using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Echidna.Core;

public class World
{
	private ISystem[] systems;
	
	private Dictionary<Type, ISystem[]> systemsDependingOn;
	
	private Entity globalEntity = new("Global Entity");
	
	private float accumulatedTime = 0;
	public float PhysicsDeltaTime { get; set; } = 1f / 60f;
	
	public World(params ISystem[] systems)
	{
		this.systems = systems;
		
		Dictionary<Type, List<ISystem>> systemsDepending = new();
		
		foreach (ISystem system in systems)
		{
			foreach (Type componentType in system.ApplicableComponentTypes)
			{
				if (!systemsDepending.ContainsKey(componentType))
					systemsDepending.Add(componentType, new List<ISystem>());
				systemsDepending[componentType].Add(system);
			}
		}
		
		systemsDependingOn = systemsDepending.ToDictionary(pair => pair.Key, pair => pair.Value.ToArray());
	}
	
	private void AddAbstractComponent(Entity entity, Component component, Type type)
	{
		if (component.Entity != null)
			throw new ArgumentException($"Component {component} already added to entity {entity}", nameof(component));
		if (entity.HasComponentType(type))
			throw new ArgumentException($"Entity {entity} already has a component of type {component}", nameof(entity));
		
		entity.AddComponent(component);
		
		if (!systemsDependingOn.TryGetValue(type, out ISystem[]? dependentSystems))
			//throw new ArgumentException($"World {this} does not have a system depending on component type {addedComponentType}", nameof(world));
			return;
		foreach (ISystem system in dependentSystems)
		{
			if (system.IsApplicableTo(entity))
				system.AddEntity(entity);
		}
	}
	
	private T AddAbstractComponent<T>(Entity entity, T component) where T : Component
	{
		AddAbstractComponent(entity, component, typeof(T));
		return component;
	}
	
	public void AddComponents(Entity entity, params EntityComponent[] components)
	{
		foreach (EntityComponent component in components)
			AddAbstractComponent(entity, component, component.GetType());
	}
	
	public void AddComponentsInstance(params EntityComponent[] components)
	{
		foreach (EntityComponent component in components)
			AddAbstractComponent(new Entity(), component, component.GetType());
	}
	
	public T AddComponent<T>(Entity entity, T component) where T : EntityComponent => AddAbstractComponent(entity, component);
	public T AddComponentInstance<T>(T component) where T : InstanceComponent => AddAbstractComponent(new Entity(), component);
	public T AddWorldComponent<T>(T component) where T : WorldComponent => AddAbstractComponent(globalEntity, component);
	
	public void Initialize()
	{
		foreach (ISystem system in systems)
			system.Initialize();
	}
	
	public void Dispose()
	{
		foreach (ISystem system in systems)
			system.Dispose();
	}
	
	public void Update(float deltaTime)
	{
		PhysicsUpdate(deltaTime);
		
		foreach (ISystem system in systems)
			system.Update(deltaTime);
	}
	
	private void PhysicsUpdate(float deltaTime)
	{
		accumulatedTime += deltaTime;
		while (accumulatedTime >= PhysicsDeltaTime)
		{
			float physicsDeltaTime = PhysicsDeltaTime;
			accumulatedTime -= physicsDeltaTime;
			foreach (ISystem system in systems)
				system.PhysicsUpdate(physicsDeltaTime);
		}
	}
	
	public void Draw()
	{
		foreach (ISystem system in systems)
			system.Draw();
	}
	
	public void MouseMove(Vector2 position, Vector2 delta)
	{
		foreach (ISystem system in systems)
			system.MouseMove(position, delta);
	}
	
	public void KeyDown(Keys key)
	{
		foreach (ISystem system in systems)
			system.KeyDown(key);
	}
	
	public void KeyUp(Keys key)
	{
		foreach (ISystem system in systems)
			system.KeyUp(key);
	}
}