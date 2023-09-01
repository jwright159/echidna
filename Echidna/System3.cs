using System.Diagnostics.Contracts;
using System.Reflection;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Echidna;

public abstract class System<T1, T2, T3> : System where T1 : Component where T2 : Component where T3 : Component
{
	private List<Entity> entities = new();
	private List<(T1, T2, T3)> componentSets = new();
	
	public IEnumerable<Type> ApplicableComponentTypes
	{
		get
		{
			yield return typeof(T1);
			yield return typeof(T2);
			yield return typeof(T3);
		}
	}
	
	protected System()
	{
		hasOnInitializeEach = IsOverridden("OnInitializeEach");
		hasOnDisposeEach = IsOverridden("OnDisposeEach");
		hasOnUpdateEach = IsOverridden("OnUpdateEach");
		hasOnDrawEach = IsOverridden("OnDrawEach");
		hasOnMouseMoveEach = IsOverridden("OnMouseMoveEach");
		hasOnKeyDownEach = IsOverridden("OnKeyDownEach");
		hasOnKeyUpEach = IsOverridden("OnKeyUpEach");
	}
	
	private bool IsOverridden(string name) => IsOverridden(GetType().GetMethod(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
	private static bool IsOverridden(MethodInfo? methodInfo) => methodInfo != null && methodInfo.GetBaseDefinition() != methodInfo;
	
	[Pure]
	public bool IsApplicableTo(Entity entity) => entity.HasComponentType<T1>() && entity.HasComponentType<T2>() && entity.HasComponentType<T3>();
	
	public void AddEntity(Entity entity)
	{
		entities.Add(entity);
		componentSets.Add((entity.GetComponent<T1>(), entity.GetComponent<T2>(), entity.GetComponent<T3>()));
	}
	
	private bool hasOnInitializeEach;
	public void Initialize()
	{
		OnInitialize(entities);
		if (hasOnInitializeEach)
			foreach ((T1 component1, T2 component2, T3 component3) in componentSets)
				OnInitializeEach(component1, component2, component3);
	}
	protected virtual void OnInitialize(IEnumerable<Entity> entities) { }
	protected virtual void OnInitializeEach(T1 component1, T2 component2, T3 component3) { }
	
	private bool hasOnDisposeEach;
	public void Dispose()
	{
		OnDispose(entities);
		if (hasOnDisposeEach)
			foreach ((T1 component1, T2 component2, T3 component3) in componentSets)
				OnDisposeEach(component1, component2, component3);
	}
	protected virtual void OnDispose(IEnumerable<Entity> entities) { }
	protected virtual void OnDisposeEach(T1 component1, T2 component2, T3 component3) { }
	
	private bool hasOnUpdateEach;
	public void Update(float deltaTime)
	{
		OnUpdate(deltaTime, entities);
		if (hasOnUpdateEach)
			foreach ((T1 component1, T2 component2, T3 component3) in componentSets)
				OnUpdateEach(deltaTime, component1, component2, component3);
	}
	protected virtual void OnUpdate(float deltaTime, IEnumerable<Entity> entities) { }
	protected virtual void OnUpdateEach(float deltaTime, T1 component1, T2 component2, T3 component3) { }
	
	private bool hasOnDrawEach;
	public void Draw()
	{
		OnDraw(entities);
		if (hasOnDrawEach)
			foreach ((T1 component1, T2 component2, T3 component3) in componentSets)
				OnDrawEach(component1, component2, component3);
	}
	protected virtual void OnDraw(IEnumerable<Entity> entities) { }
	protected virtual void OnDrawEach(T1 component1, T2 component2, T3 component3) { }
	
	private bool hasOnMouseMoveEach;
	public void MouseMove(Vector2 position, Vector2 delta)
	{
		OnMouseMove(position, delta, entities);
		if (hasOnMouseMoveEach)
			foreach ((T1 component1, T2 component2, T3 component3) in componentSets)
				OnMouseMoveEach(position, delta, component1, component2, component3);
	}
	protected virtual void OnMouseMove(Vector2 position, Vector2 delta, IEnumerable<Entity> entities) { }
	protected virtual void OnMouseMoveEach(Vector2 position, Vector2 delta, T1 component1, T2 component2, T3 component3) { }
	
	private bool hasOnKeyDownEach;
	public void KeyDown(Keys key)
	{
		OnKeyDown(key, entities);
		if (hasOnKeyDownEach)
			foreach ((T1 component1, T2 component2, T3 component3) in componentSets)
				OnKeyDownEach(key, component1, component2, component3);
	}
	protected virtual void OnKeyDown(Keys key, IEnumerable<Entity> entities) { }
	protected virtual void OnKeyDownEach(Keys key, T1 component1, T2 component2, T3 component3) { }
	
	private bool hasOnKeyUpEach;
	public void KeyUp(Keys key)
	{
		OnKeyUp(key, entities);
		if (hasOnKeyUpEach)
			foreach ((T1 component1, T2 component2, T3 component3) in componentSets)
				OnKeyUpEach(key, component1, component2, component3);
	}
	protected virtual void OnKeyUp(Keys key, IEnumerable<Entity> entities) { }
	protected virtual void OnKeyUpEach(Keys key, T1 component1, T2 component2, T3 component3) { }
}