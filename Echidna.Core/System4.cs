using System.Diagnostics.Contracts;
using System.Reflection;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Echidna.Core;

public abstract class System<T1, T2, T3, T4> : ISystem where T1 : Component where T2 : Component where T3 : Component where T4 : Component
{
	private List<(T1, T2, T3, T4)> componentSets = new();
	
	public IEnumerable<Type> ApplicableComponentTypes
	{
		get
		{
			yield return typeof(T1);
			yield return typeof(T2);
			yield return typeof(T3);
			yield return typeof(T4);
		}
	}
	
	protected System()
	{
		hasOnInitializeEach = IsOverridden("OnInitializeEach");
		hasOnDisposeEach = IsOverridden("OnDisposeEach");
		hasOnUpdateEach = IsOverridden("OnUpdateEach");
		hasOnPhysicsUpdateEach = IsOverridden("OnPhysicsUpdateEach");
		hasOnDrawEach = IsOverridden("OnDrawEach");
		hasOnMouseMoveEach = IsOverridden("OnMouseMoveEach");
		hasOnKeyDownEach = IsOverridden("OnKeyDownEach");
		hasOnKeyUpEach = IsOverridden("OnKeyUpEach");
	}
	
	private bool IsOverridden(string name) => IsOverridden(GetType().GetMethod(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
	private static bool IsOverridden(MethodInfo? methodInfo) => methodInfo != null && methodInfo.GetBaseDefinition() != methodInfo;
	
	[Pure]
	public bool IsApplicableTo(Entity entity) => entity.HasComponentType<T1>() && entity.HasComponentType<T2>() && entity.HasComponentType<T3>() && entity.HasComponentType<T4>();
	
	public void AddEntity(Entity entity) => componentSets.Add((entity.GetComponent<T1>(), entity.GetComponent<T2>(), entity.GetComponent<T3>(), entity.GetComponent<T4>()));
	
	private bool hasOnInitializeEach;
	public void Initialize()
	{
		OnInitialize(componentSets);
		if (hasOnInitializeEach)
			foreach ((T1 component1, T2 component2, T3 component3, T4 component4) in componentSets)
				OnInitializeEach(component1, component2, component3, component4);
	}
	protected virtual void OnInitialize(IEnumerable<(T1, T2, T3, T4)> componentSets) { }
	protected virtual void OnInitializeEach(T1 component1, T2 component2, T3 component3, T4 component4) { }
	
	private bool hasOnDisposeEach;
	public void Dispose()
	{
		OnDispose(componentSets);
		if (hasOnDisposeEach)
			foreach ((T1 component1, T2 component2, T3 component3, T4 component4) in componentSets)
				OnDisposeEach(component1, component2, component3, component4);
	}
	protected virtual void OnDispose(IEnumerable<(T1, T2, T3, T4)> componentSets) { }
	protected virtual void OnDisposeEach(T1 component1, T2 component2, T3 component3, T4 component4) { }
	
	private bool hasOnPhysicsUpdateEach;
	public void PhysicsUpdate(float deltaTime)
	{
		OnPhysicsUpdate(deltaTime, componentSets);
		if (hasOnPhysicsUpdateEach)
			foreach ((T1 component1, T2 component2, T3 component3, T4 component4) in componentSets)
				OnPhysicsUpdateEach(deltaTime, component1, component2, component3, component4);
	}
	protected virtual void OnPhysicsUpdate(float deltaTime, IEnumerable<(T1, T2, T3, T4)> componentSets) { }
	protected virtual void OnPhysicsUpdateEach(float deltaTime, T1 component1, T2 component2, T3 component3, T4 component4) { }
	
	private bool hasOnUpdateEach;
	public void Update(float deltaTime)
	{
		OnUpdate(deltaTime, componentSets);
		if (hasOnUpdateEach)
			foreach ((T1 component1, T2 component2, T3 component3, T4 component4) in componentSets)
				OnUpdateEach(deltaTime, component1, component2, component3, component4);
	}
	protected virtual void OnUpdate(float deltaTime, IEnumerable<(T1, T2, T3, T4)> componentSets) { }
	protected virtual void OnUpdateEach(float deltaTime, T1 component1, T2 component2, T3 component3, T4 component4) { }
	
	private bool hasOnDrawEach;
	public void Draw()
	{
		OnDraw(componentSets);
		if (hasOnDrawEach)
			foreach ((T1 component1, T2 component2, T3 component3, T4 component4) in componentSets)
				OnDrawEach(component1, component2, component3, component4);
	}
	protected virtual void OnDraw(IEnumerable<(T1, T2, T3, T4)> componentSets) { }
	protected virtual void OnDrawEach(T1 component1, T2 component2, T3 component3, T4 component4) { }
	
	private bool hasOnMouseMoveEach;
	public void MouseMove(Vector2 position, Vector2 delta)
	{
		OnMouseMove(position, delta, componentSets);
		if (hasOnMouseMoveEach)
			foreach ((T1 component1, T2 component2, T3 component3, T4 component4) in componentSets)
				OnMouseMoveEach(position, delta, component1, component2, component3, component4);
	}
	protected virtual void OnMouseMove(Vector2 position, Vector2 delta, IEnumerable<(T1, T2, T3, T4)> componentSets) { }
	protected virtual void OnMouseMoveEach(Vector2 position, Vector2 delta, T1 component1, T2 component2, T3 component3, T4 component4) { }
	
	private bool hasOnKeyDownEach;
	public void KeyDown(Keys key)
	{
		OnKeyDown(key, componentSets);
		if (hasOnKeyDownEach)
			foreach ((T1 component1, T2 component2, T3 component3, T4 component4) in componentSets)
				OnKeyDownEach(key, component1, component2, component3, component4);
	}
	protected virtual void OnKeyDown(Keys key, IEnumerable<(T1, T2, T3, T4)> componentSets) { }
	protected virtual void OnKeyDownEach(Keys key, T1 component1, T2 component2, T3 component3, T4 component4) { }
	
	private bool hasOnKeyUpEach;
	public void KeyUp(Keys key)
	{
		OnKeyUp(key, componentSets);
		if (hasOnKeyUpEach)
			foreach ((T1 component1, T2 component2, T3 component3, T4 component4) in componentSets)
				OnKeyUpEach(key, component1, component2, component3, component4);
	}
	protected virtual void OnKeyUp(Keys key, IEnumerable<(T1, T2, T3, T4)> componentSets) { }
	protected virtual void OnKeyUpEach(Keys key, T1 component1, T2 component2, T3 component3, T4 component4) { }
}