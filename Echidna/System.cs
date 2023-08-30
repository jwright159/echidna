using System.Reflection;
using JetBrains.Annotations;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Echidna;

public abstract class System
{
	private Type[] componentTypes;
	public IEnumerable<Type> ComponentTypes => componentTypes;
	
	private HashSet<Entity> entities = new();
	public IEnumerable<Entity> Entities => entities;
	
	protected System(params Type[] componentTypes)
	{
		if (componentTypes.Length == 0)
			throw new ArgumentException($"System {this} was given no component types");
		this.componentTypes = componentTypes;
		
		Type[] deltaTimeTypes = componentTypes.Prepend(typeof(float)).ToArray();
		onUpdateEaches = GetType().GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
			.Where(method => method.GetCustomAttribute<UpdateEachAttribute>() != null)
			.Where(method => method.GetParameters().Select(parameter => parameter.ParameterType).SequenceEqual(deltaTimeTypes))
			.ToArray();
	}
	
	public bool IsApplicableTo(Entity entity) => componentTypes.All(entity.HasComponentType);
	
	public void AddEntity(Entity entity) => entities.Add(entity);
	
	public virtual void OnInitialize() { }
	public virtual void OnDispose() { }
	
	private MethodInfo[] onUpdateEaches;
	public void OnUpdate(float deltaTime)
	{
		foreach (object[] parameters in Entities.Select(entity => componentTypes.Select<Type, object>(entity.GetComponent).Prepend(deltaTime).ToArray()))
		foreach (MethodInfo onUpdateEach in onUpdateEaches)
			onUpdateEach.Invoke(this, parameters);
	}
	public virtual void OnDraw(float deltaTime) { }
	
	public virtual void OnMouseMove(Vector2 position, Vector2 delta) { }
	public virtual void OnKeyDown(Keys key) { }
	public virtual void OnKeyUp(Keys key) { }
}

[MeansImplicitUse, AttributeUsage(AttributeTargets.Method)]
public class UpdateEachAttribute : Attribute
{
	
}