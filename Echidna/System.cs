using System.Reflection;
using JetBrains.Annotations;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Echidna;

public abstract class System
{
	private Type[] componentTypes;
	public IEnumerable<Type> ComponentTypes => componentTypes;
	
	private List<object[]> componentSets = new();
	private Dictionary<Type, MethodInfo[]> methodSets = new();
	
	protected System(params Type[] componentTypes)
	{
		if (componentTypes.Length == 0)
			throw new ArgumentException($"System {this} was given no component types");
		this.componentTypes = componentTypes;
		
		AddAnnotatedMethods<InitializeEachAttribute>();
		AddAnnotatedMethods<DisposeEachAttribute>();
		AddAnnotatedMethods<UpdateEachAttribute>(typeof(float));
		AddAnnotatedMethods<DrawEachAttribute>(typeof(float));
		AddAnnotatedMethods<MouseMoveEachAttribute>(typeof(Vector2), typeof(Vector2));
		AddAnnotatedMethods<KeyDownEachAttribute>(typeof(Keys));
		AddAnnotatedMethods<KeyUpEachAttribute>(typeof(Keys));
	}
	
	private void AddAnnotatedMethods<T>(params Type[] otherParameterTypes) where T : Attribute
	{
		methodSets[typeof(T)] = GetAnnotatedMethods<T>(otherParameterTypes.Length > 0 ? otherParameterTypes.Concat(componentTypes).ToArray() : componentTypes);
	}
	
	private MethodInfo[] GetAnnotatedMethods<T>(Type[] parameterSignature) where T : Attribute
	{
		MethodInfo[] methods = GetType().GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
			.Where(method => method.GetCustomAttribute<T>() != null)
			.ToArray();
		foreach (MethodInfo method in methods)
			if (!method.GetParameters().Select(parameter => parameter.ParameterType).SequenceEqual(parameterSignature))
				throw new ArgumentException($"System {this} method {method} with attribute {typeof(T)} does not have the right parameters ({string.Join(", ", parameterSignature.AsEnumerable())})");
		return methods;
	}
	
	private void InvokeAnnotatedMethods<T>(params object[] otherParameters) where T : Attribute
	{
		if (methodSets[typeof(T)].Length == 0) return;
		
		IEnumerable<object[]> parameterSets = componentSets;
		if (otherParameters.Length > 0)
			parameterSets = BuildNewParameters(componentSets, otherParameters);
		
		foreach (object[] parameters in parameterSets)
		foreach (MethodInfo method in methodSets[typeof(T)])
			method.Invoke(this, parameters);
	}
	
	private static IEnumerable<object[]> BuildNewParameters(List<object[]> componentSets, object[] otherParameters)
	{
		foreach (object[] components in componentSets)
		{
			object[] parameters = new object[otherParameters.Length + components.Length];
			Array.Copy(otherParameters, 0, parameters, 0, otherParameters.Length);
			Array.Copy(components, 0, parameters, otherParameters.Length, components.Length);
			yield return parameters;
		}
	}
	
	public bool IsApplicableTo(Entity entity) => componentTypes.All(entity.HasComponentType);
	
	public void AddEntity(Entity entity) => componentSets.Add(componentTypes.Select(entity.GetComponent).Cast<object>().ToArray());
	
	[MeansImplicitUse, AttributeUsage(AttributeTargets.Method)]
	protected class InitializeEachAttribute : Attribute { }
	public void OnInitialize() => InvokeAnnotatedMethods<InitializeEachAttribute>();
	
	[MeansImplicitUse, AttributeUsage(AttributeTargets.Method)]
	protected class DisposeEachAttribute : Attribute { }
	public void OnDispose() => InvokeAnnotatedMethods<DisposeEachAttribute>();
	
	[MeansImplicitUse, AttributeUsage(AttributeTargets.Method)]
	protected class UpdateEachAttribute : Attribute { }
	public void OnUpdate(float deltaTime) => InvokeAnnotatedMethods<UpdateEachAttribute>(deltaTime);
	
	[MeansImplicitUse, AttributeUsage(AttributeTargets.Method)]
	protected class DrawEachAttribute : Attribute { }
	public void OnDraw(float deltaTime) => InvokeAnnotatedMethods<DrawEachAttribute>(deltaTime);
	
	[MeansImplicitUse, AttributeUsage(AttributeTargets.Method)]
	protected class MouseMoveEachAttribute : Attribute { }
	public void OnMouseMove(Vector2 position, Vector2 delta) => InvokeAnnotatedMethods<MouseMoveEachAttribute>(position, delta);
	
	[MeansImplicitUse, AttributeUsage(AttributeTargets.Method)]
	protected class KeyDownEachAttribute : Attribute { }
	public void OnKeyDown(Keys key) => InvokeAnnotatedMethods<KeyDownEachAttribute>(key);
	
	[MeansImplicitUse, AttributeUsage(AttributeTargets.Method)]
	protected class KeyUpEachAttribute : Attribute { }
	public void OnKeyUp(Keys key) => InvokeAnnotatedMethods<KeyUpEachAttribute>(key);
}