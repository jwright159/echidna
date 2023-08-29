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
		foreach (Type componentType in system.ComponentTypes)
		{
			if (!systemsDepending.ContainsKey(componentType))
				systemsDepending.Add(componentType, new List<System>());
			systemsDepending[componentType].Add(system);
		}
		
		systemsDependingOn = systemsDepending.ToDictionary(pair => pair.Key, pair => pair.Value.ToArray());
	}
	
	public void AddComponent(Entity entity, Component component)
	{
		if (entity.HasComponentType(component.GetType()))
			throw new InvalidOperationException($"Entity {entity} already has a component of type {component}");
		entity.AddComponent(component);
		
		Type addedComponentType = component.GetType();
		
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
	
	public void Draw()
	{
		foreach (System system in systems)
			system.OnDraw();
	}
}