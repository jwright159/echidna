namespace Echidna.Input;

public class InputGroup : Component
{
	public readonly List<InputAction> actions = new();
	
	public InputGroup(params InputAction[] actions)
	{
		this.actions.AddRange(actions);
	}
}