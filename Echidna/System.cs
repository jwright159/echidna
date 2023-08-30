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
	private Dictionary<Type, MethodSet> methodSets = new();
	
	protected System(params Type[] componentTypes)
	{
		if (componentTypes.Length == 0)
			throw new ArgumentException($"System {this} was given no component types");
		this.componentTypes = componentTypes;
		
		AddAnnotatedMethods<InitializeEachAttribute>();
		AddAnnotatedMethods<DisposeEachAttribute>();
		AddAnnotatedMethods<UpdateEachAttribute>();
		AddAnnotatedMethods<DrawEachAttribute>();
		AddAnnotatedMethods<MouseMoveEachAttribute>(typeof(Vector2), typeof(Vector2));
		AddAnnotatedMethods<KeyDownEachAttribute>(typeof(Keys));
		AddAnnotatedMethods<KeyUpEachAttribute>(typeof(Keys));
	}
	
	private void AddAnnotatedMethods<T>(params Type[] otherParameterTypes) where T : Attribute
	{
		Type[] parameterTypes = otherParameterTypes.Length > 0 ? otherParameterTypes.Concat(componentTypes).ToArray() : componentTypes;
		methodSets[typeof(T)] = new MethodSet(otherParameterTypes.Length, componentTypes.Length, GetAnnotatedMethods<T>(parameterTypes));
	}
	
	private MethodInfo[] GetAnnotatedMethods<T>(Type[] parameterSignature) where T : Attribute
	{
		MethodInfo[] methods = GetType().GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
			.Where(method => method.GetCustomAttribute<T>() != null)
			.ToArray();
		foreach (MethodInfo method in methods)
			if (!method.GetParameters().Select(parameter => parameter.ParameterType).SequenceEqual(parameterSignature))
				throw new ArgumentException($"System {this} method {method} with attribute {typeof(T)} does not have the right parameters ({string.Join(", ", parameterSignature.AsEnumerable())})");
		return methods;
	}
	
	[Pure]
	private MethodSet GetMethodSet<T>() where T : Attribute => methodSets[typeof(T)];
	
	[Pure]
	public bool IsApplicableTo(Entity entity) => componentTypes.All(entity.HasComponentType);
	
	public void AddEntity(Entity entity) => componentSets.Add(componentTypes.Select(entity.GetComponent).Cast<object>().ToArray());
	
	[MeansImplicitUse, AttributeUsage(AttributeTargets.Method)]
	protected class InitializeEachAttribute : Attribute { }
	public void OnInitialize() => GetMethodSet<InitializeEachAttribute>().Invoke(componentSets);
	
	[MeansImplicitUse, AttributeUsage(AttributeTargets.Method)]
	protected class DisposeEachAttribute : Attribute { }
	public void OnDispose() => GetMethodSet<DisposeEachAttribute>().Invoke(componentSets);
	
	[MeansImplicitUse, AttributeUsage(AttributeTargets.Method)]
	protected class UpdateEachAttribute : Attribute { }
	public void OnUpdate() => GetMethodSet<UpdateEachAttribute>().Invoke(componentSets);
	
	[MeansImplicitUse, AttributeUsage(AttributeTargets.Method)]
	protected class DrawEachAttribute : Attribute { }
	public void OnDraw() => GetMethodSet<DrawEachAttribute>().Invoke(componentSets);
	
	[MeansImplicitUse, AttributeUsage(AttributeTargets.Method)]
	protected class MouseMoveEachAttribute : Attribute { }
	public void OnMouseMove(Vector2 position, Vector2 delta) => GetMethodSet<MouseMoveEachAttribute>().With(0, position).With(1, delta).Invoke(componentSets);
	
	[MeansImplicitUse, AttributeUsage(AttributeTargets.Method)]
	protected class KeyDownEachAttribute : Attribute { }
	public void OnKeyDown(Keys key) => GetMethodSet<KeyDownEachAttribute>().With(0, key).Invoke(componentSets);
	
	[MeansImplicitUse, AttributeUsage(AttributeTargets.Method)]
	protected class KeyUpEachAttribute : Attribute { }
	public void OnKeyUp(Keys key) => GetMethodSet<KeyUpEachAttribute>().With(0, key).Invoke(componentSets);
	
	private class MethodSet
	{
		private readonly int numOtherParameters;
		private readonly object[] parameters;
		private readonly MethodInfo[] methods;
		
		public MethodSet(int numOtherParameters, int numComponents, MethodInfo[] methods)
		{
			this.numOtherParameters = numOtherParameters;
			parameters = new object[numOtherParameters + numComponents];
			this.methods = methods;
		}
		
		public MethodSet With(int index, object parameter)
		{
			parameters[index] = parameter;
			return this;
		}
		
		public void Invoke(List<object[]> componentSets)
		{
			if (methods.Length == 0) return;
		
			foreach (object[] components in componentSets)
			{
				Array.Copy(components, 0, parameters, numOtherParameters, components.Length);
				foreach (MethodInfo method in methods)
					method.Invoke(null, parameters);
			}
		}
	}
}