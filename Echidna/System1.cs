using System.Diagnostics.Contracts;
using System.Reflection;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Echidna;

public abstract class System<T1> : System where T1 : Component
{
	private List<T1> componentSets = new();
	
	public IEnumerable<Type> ApplicableComponentTypes
	{
		get
		{
			yield return typeof(T1);
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
	public bool IsApplicableTo(Entity entity) => entity.HasComponentType<T1>();
	
	public void AddEntity(Entity entity) => componentSets.Add(entity.GetComponent<T1>());
	
	private bool hasOnInitializeEach;
	public void Initialize()
	{
		OnInitialize(componentSets);
		if (hasOnInitializeEach)
			foreach (T1 component in componentSets)
				OnInitializeEach(component);
	}
	protected virtual void OnInitialize(IEnumerable<T1> componentSets) { }
	protected virtual void OnInitializeEach(T1 component) { }
	
	private bool hasOnDisposeEach;
	public void Dispose()
	{
		OnDispose(componentSets);
		if (hasOnDisposeEach)
			foreach (T1 component in componentSets)
				OnDisposeEach(component);
	}
	protected virtual void OnDispose(IEnumerable<T1> componentSets) { }
	protected virtual void OnDisposeEach(T1 component) { }
	
	private bool hasOnUpdateEach;
	public void Update(float deltaTime)
	{
		OnUpdate(deltaTime, componentSets);
		if (hasOnUpdateEach)
			foreach (T1 component in componentSets)
				OnUpdateEach(deltaTime, component);
	}
	protected virtual void OnUpdate(float deltaTime, IEnumerable<T1> componentSets) { }
	protected virtual void OnUpdateEach(float deltaTime, T1 component) { }
	
	private bool hasOnDrawEach;
	public void Draw()
	{
		OnDraw(componentSets);
		if (hasOnDrawEach)
			foreach (T1 component in componentSets)
				OnDrawEach(component);
	}
	protected virtual void OnDraw(IEnumerable<T1> componentSets) { }
	protected virtual void OnDrawEach(T1 component) { }
	
	private bool hasOnMouseMoveEach;
	public void MouseMove(Vector2 position, Vector2 delta)
	{
		OnMouseMove(position, delta, componentSets);
		if (hasOnMouseMoveEach)
			foreach (T1 component in componentSets)
				OnMouseMoveEach(position, delta, component);
	}
	protected virtual void OnMouseMove(Vector2 position, Vector2 delta, IEnumerable<T1> componentSets) { }
	protected virtual void OnMouseMoveEach(Vector2 position, Vector2 delta, T1 component) { }
	
	private bool hasOnKeyDownEach;
	public void KeyDown(Keys key)
	{
		OnKeyDown(key, componentSets);
		if (hasOnKeyDownEach)
			foreach (T1 component in componentSets)
				OnKeyDownEach(key, component);
	}
	protected virtual void OnKeyDown(Keys key, IEnumerable<T1> componentSets) { }
	protected virtual void OnKeyDownEach(Keys key, T1 component) { }
	
	private bool hasOnKeyUpEach;
	public void KeyUp(Keys key)
	{
		OnKeyUp(key, componentSets);
		if (hasOnKeyUpEach)
			foreach (T1 component in componentSets)
				OnKeyUpEach(key, component);
	}
	protected virtual void OnKeyUp(Keys key, IEnumerable<T1> componentSets) { }
	protected virtual void OnKeyUpEach(Keys key, T1 component) { }
}