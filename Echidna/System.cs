using System.Reflection;
using Fasterflect;
using JetBrains.Annotations;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Echidna;

public abstract class System
{
	private Type[] allComponentTypes;
	private Type[] eachComponentTypes;
	public IEnumerable<Type> EachComponentTypes => eachComponentTypes;
	
	private List<Entity> entities = new();
	private List<object[]> componentSets = new();
	private Dictionary<Type, MethodSet> methodSets = new();
	
	protected System(params Type[] eachComponentTypes)
	{
		if (eachComponentTypes.Length == 0)
			throw new ArgumentException($"System {this} was given no component types");
		
		allComponentTypes = new []{ typeof(List<Entity>) };
		this.eachComponentTypes = eachComponentTypes;
		
		AddAnnotatedMethods<InitializeAllAttribute, InitializeEachAttribute>();
		AddAnnotatedMethods<DisposeAllAttribute, DisposeEachAttribute>();
		AddAnnotatedMethods<UpdateAllAttribute, UpdateEachAttribute>();
		AddAnnotatedMethods<DrawAllAttribute, DrawEachAttribute>();
		AddAnnotatedMethods<MouseMoveAllAttribute, MouseMoveEachAttribute>(typeof(Vector2), typeof(Vector2));
		AddAnnotatedMethods<KeyDownAllAttribute, KeyDownEachAttribute>(typeof(Keys));
		AddAnnotatedMethods<KeyUpAllAttribute, KeyUpEachAttribute>(typeof(Keys));
	}
	
	private void AddAnnotatedMethods<TAll, TEach>(params Type[] otherParameterTypes) where TAll : Attribute where TEach : Attribute
	{
		Type[] allParameterTypes = otherParameterTypes.Length > 0 ? otherParameterTypes.Concat(allComponentTypes).ToArray() : allComponentTypes;
		Type[] eachParameterTypes = otherParameterTypes.Length > 0 ? otherParameterTypes.Concat(eachComponentTypes).ToArray() : eachComponentTypes;
		methodSets[typeof(TEach)] = new MethodSet(otherParameterTypes.Length, eachComponentTypes.Length, GetAnnotatedMethods<TAll>(allParameterTypes), GetAnnotatedMethods<TEach>(eachParameterTypes));
	}
	
	private MethodInvoker[] GetAnnotatedMethods<T>(Type[] parameterSignature) where T : Attribute
	{
		IList<MethodInfo> methods = GetType().MethodsWith(Flags.StaticInstanceAnyVisibility, typeof(T));
		foreach (MethodInfo method in methods)
			if (!method.Parameters().Select(parameter => parameter.ParameterType).SequenceEqual(parameterSignature))
				throw new ArgumentException($"System {this} method {method} with attribute {typeof(T)} does not have the right parameters ({string.Join(", ", parameterSignature.AsEnumerable())})");
		return methods.Select(method => method.DelegateForCallMethod()).ToArray();
	}
	
	[Pure]
	private MethodSet GetMethodSet<T>() where T : Attribute => methodSets[typeof(T)];
	
	[Pure]
	public bool IsApplicableTo(Entity entity) => eachComponentTypes.All(entity.HasComponentType);
	
	public void AddEntity(Entity entity)
	{
		entities.Add(entity);
		componentSets.Add(eachComponentTypes.Select(entity.GetComponent).Cast<object>().ToArray());
	}
	
	[MeansImplicitUse, AttributeUsage(AttributeTargets.Method)]
	protected class InitializeAllAttribute : Attribute { }
	[MeansImplicitUse, AttributeUsage(AttributeTargets.Method)]
	protected class InitializeEachAttribute : Attribute { }
	public void OnInitialize() => GetMethodSet<InitializeEachAttribute>().Invoke(this);
	
	[MeansImplicitUse, AttributeUsage(AttributeTargets.Method)]
	protected class DisposeAllAttribute : Attribute { }
	[MeansImplicitUse, AttributeUsage(AttributeTargets.Method)]
	protected class DisposeEachAttribute : Attribute { }
	public void OnDispose() => GetMethodSet<DisposeEachAttribute>().Invoke(this);
	
	[MeansImplicitUse, AttributeUsage(AttributeTargets.Method)]
	protected class UpdateAllAttribute : Attribute { }
	[MeansImplicitUse, AttributeUsage(AttributeTargets.Method)]
	protected class UpdateEachAttribute : Attribute { }
	public void OnUpdate() => GetMethodSet<UpdateEachAttribute>().Invoke(this);
	
	[MeansImplicitUse, AttributeUsage(AttributeTargets.Method)]
	protected class DrawAllAttribute : Attribute { }
	[MeansImplicitUse, AttributeUsage(AttributeTargets.Method)]
	protected class DrawEachAttribute : Attribute { }
	public void OnDraw() => GetMethodSet<DrawEachAttribute>().Invoke(this);
	
	[MeansImplicitUse, AttributeUsage(AttributeTargets.Method)]
	protected class MouseMoveAllAttribute : Attribute { }
	[MeansImplicitUse, AttributeUsage(AttributeTargets.Method)]
	protected class MouseMoveEachAttribute : Attribute { }
	public void OnMouseMove(Vector2 position, Vector2 delta) => GetMethodSet<MouseMoveEachAttribute>().With(0, position).With(1, delta).Invoke(this);
	
	[MeansImplicitUse, AttributeUsage(AttributeTargets.Method)]
	protected class KeyDownAllAttribute : Attribute { }
	[MeansImplicitUse, AttributeUsage(AttributeTargets.Method)]
	protected class KeyDownEachAttribute : Attribute { }
	public void OnKeyDown(Keys key) => GetMethodSet<KeyDownEachAttribute>().With(0, key).Invoke(this);
	
	[MeansImplicitUse, AttributeUsage(AttributeTargets.Method)]
	protected class KeyUpAllAttribute : Attribute { }
	[MeansImplicitUse, AttributeUsage(AttributeTargets.Method)]
	protected class KeyUpEachAttribute : Attribute { }
	public void OnKeyUp(Keys key) => GetMethodSet<KeyUpEachAttribute>().With(0, key).Invoke(this);
	
	private class MethodSet
	{
		private readonly int numOtherParameters;
		private readonly object[] allParameters;
		private readonly MethodInvoker[] allMethods;
		private readonly object[] eachParameters;
		private readonly MethodInvoker[] eachMethods;
		
		public MethodSet(int numOtherParameters, int numComponents, MethodInvoker[] allMethods, MethodInvoker[] eachMethods)
		{
			this.numOtherParameters = numOtherParameters;
			allParameters = new object[numOtherParameters + 1];
			this.allMethods = allMethods;
			eachParameters = new object[numOtherParameters + numComponents];
			this.eachMethods = eachMethods;
		}
		
		public MethodSet With(int index, object parameter)
		{
			eachParameters[index] = parameter;
			return this;
		}
		
		public void Invoke(System system) => Invoke(system, system.entities, system.componentSets);
		public void Invoke(System system, List<Entity> entities, List<object[]> componentSets)
		{
			InvokeAll(system, entities);
			InvokeEach(system, componentSets);
		}
		
		private void InvokeAll(System system, List<Entity> entities)
		{
			if (allMethods.Length == 0) return;
			
			allParameters[numOtherParameters] = entities;
			foreach (MethodInvoker allMethod in allMethods)
				allMethod.Invoke(system, allParameters);
		}
		
		private void InvokeEach(System system, List<object[]> componentSets)
		{
			if (eachMethods.Length == 0) return;
			
			InvokeEachLoop(system, componentSets);
		}
		
		private void InvokeEachLoop(System system, List<object[]> componentSets)
		{
			foreach (object[] components in componentSets)
				InvokeEachLoopInterior(system, components);
		}
		
		private void InvokeEachLoopInterior(System system, object[] components)
		{
			InvokeEachLoopInteriorArrayCopy(components);
			InvokeEachLoopInteriorLoop(system);
		}
		
		private void InvokeEachLoopInteriorArrayCopy(object[] components)
		{
			Array.Copy(components, 0, eachParameters, numOtherParameters, components.Length);
		}
		
		private void InvokeEachLoopInteriorLoop(System system)
		{
			foreach (MethodInvoker eachMethod in eachMethods)
				InvokeEachLoopInteriorLoopInterior(system, eachMethod);
		}
		
		private void InvokeEachLoopInteriorLoopInterior(System system, MethodInvoker eachMethod)
		{
			eachMethod.Invoke(system, eachParameters);
		}
	}
}