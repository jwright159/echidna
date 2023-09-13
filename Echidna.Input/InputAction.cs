namespace Echidna.Input;

public class InputAction
{
	private Dictionary<object, InputTrigger[]> triggers;
	
	public Action<object> Action { get; }
	
	public InputAction(Action<object> action, params InputTrigger[] triggers)
	{
		Action = action;
		
		Dictionary<object, List<InputTrigger>> triggerMap = new();
		foreach (InputTrigger trigger in triggers)
		foreach (object inputType in trigger.InputTypes)
		{
			if (!triggerMap.ContainsKey(inputType))
				triggerMap.Add(inputType, new List<InputTrigger>());
			triggerMap[inputType].Add(trigger);
		}
		
		this.triggers = triggerMap.ToDictionary(pair => pair.Key, pair => pair.Value.ToArray());
	}
	
	public bool HasTriggerFor(object type) => triggers.ContainsKey(type);
	public IEnumerable<InputTrigger> this[object type] => triggers[type];
	public IEnumerable<object> InputTypes => triggers.Keys;
}

public class InputAction<T> : InputAction where T : notnull
{
	public InputAction(Action<T> action, params InputTrigger<T>[] triggers) : base(value => action((T)value), triggers.Cast<InputTrigger>().ToArray()) { }
}