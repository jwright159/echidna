namespace Echidna.Input;

public class InputGroup : Component
{
	private Dictionary<object, InputAction[]> actions;
	
	public InputGroup(params InputAction[] actions)
	{
		Dictionary<object, List<InputAction>> actionMap = new();
		foreach (InputAction action in actions)
		foreach (object inputTypes in action.InputTypes)
		{
			if (!actionMap.ContainsKey(inputTypes))
				actionMap.Add(inputTypes, new List<InputAction>());
			actionMap[inputTypes].Add(action);
		}
		
		this.actions = actionMap.ToDictionary(pair => pair.Key, pair => pair.Value.ToArray());
	}
	
	public bool HasActionFor(object type) => actions.ContainsKey(type);
	public IEnumerable<InputAction> this[object type] => actions[type];
	public IEnumerable<object> InputTypes => actions.Keys;
}