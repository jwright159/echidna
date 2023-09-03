using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Echidna;

public class World
{
	private System[] systems;
	
	private Dictionary<Type, System[]> systemsDependingOn;
	
	private Entity globalEntity = new();
	
	private float accumulatedTime = 0;
	public float PhysicsDeltaTime { get; set; } = 1f / 60f;
	
	public World(params System[] systems)
	{
		this.systems = systems;
		
		Dictionary<Type, List<System>> systemsDepending = new();
		
		foreach (System system in systems)
		{
			foreach (Type componentType in system.ApplicableComponentTypes)
			{
				if (!systemsDepending.ContainsKey(componentType))
					systemsDepending.Add(componentType, new List<System>());
				systemsDepending[componentType].Add(system);
			}
		}
		
		systemsDependingOn = systemsDepending.ToDictionary(pair => pair.Key, pair => pair.Value.ToArray());
	}
	
	public void AddComponent<T>(Entity entity, T component) where T : Component
	{
		Type addedComponentType = typeof(T);
		if (entity.HasComponentType(addedComponentType))
			throw new InvalidOperationException($"Entity {entity} already has a component of type {component}");
		if (!systemsDependingOn.ContainsKey(addedComponentType))
			throw new InvalidOperationException($"World {this} does not have a system depending on component type {addedComponentType}");
		
		entity.AddComponent(component);
		foreach (System system in systemsDependingOn[addedComponentType])
		{
			if (system.IsApplicableTo(entity))
				system.AddEntity(entity);
		}
	}
	
	public void AddSingletonComponent<T>(T component) where T : Component => AddComponent(globalEntity, component);
	
	public void Initialize()
	{
		foreach (System system in systems)
			system.Initialize();
	}
	
	public void Dispose()
	{
		foreach (System system in systems)
			system.Dispose();
	}
	
	public void Update(float deltaTime)
	{
		PhysicsUpdate(deltaTime);
		
		foreach (System system in systems)
			system.Update(deltaTime);
	}
	
	private void PhysicsUpdate(float deltaTime)
	{
		accumulatedTime += deltaTime;
		while (accumulatedTime >= PhysicsDeltaTime)
		{
			float physicsDeltaTime = PhysicsDeltaTime;
			accumulatedTime -= physicsDeltaTime;
			foreach (System system in systems)
				system.PhysicsUpdate(physicsDeltaTime);
		}
	}
	
	public void Draw()
	{
		foreach (System system in systems)
			system.Draw();
	}
	
	public void MouseMove(Vector2 position, Vector2 delta)
	{
		foreach (System system in systems)
			system.MouseMove(position, delta);
	}
	
	public void KeyDown(Keys key)
	{
		foreach (System system in systems)
			system.KeyDown(key);
	}
	
	public void KeyUp(Keys key)
	{
		foreach (System system in systems)
			system.KeyUp(key);
	}
}