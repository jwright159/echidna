namespace Echidna.Input;

public class InputAction
{
	public readonly List<InputTrigger> triggers = new();
	
	public Action<object> Action { get; }
	
	public InputAction(Action<object> action, params InputTrigger[] triggers)
	{
		Action = action;
		this.triggers.AddRange(triggers);
	}
}

public class InputAction<T> : InputAction
{
	public InputAction(Action<T> action, params InputTrigger[] triggers) : base(value => action((T)value), triggers) { }
}