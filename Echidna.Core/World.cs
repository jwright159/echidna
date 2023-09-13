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
	
	public void AddComponent<T>(Entity entity, T component) where T : Component
	{
		if (component.Entity != null)
			throw new ArgumentException($"Component {component} already added to entity {entity}", nameof(component));
		Type addedComponentType = typeof(T);
		if (entity.HasComponentType(addedComponentType))
			throw new ArgumentException($"Entity {entity} already has a component of type {component}", nameof(entity));
		
		entity.AddComponent(component);
		
		if (!systemsDependingOn.ContainsKey(addedComponentType))
			//throw new ArgumentException($"World {this} does not have a system depending on component type {addedComponentType}", nameof(world));
			return;
		foreach (ISystem system in systemsDependingOn[addedComponentType])
		{
			if (system.IsApplicableTo(entity))
				system.AddEntity(entity);
		}
	}
	
	public void AddSingletonComponent<T>(T component) where T : Component => AddComponent(globalEntity, component);
	
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