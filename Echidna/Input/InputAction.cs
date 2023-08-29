namespace Echidna.Input;

public class InputAction
{
	public readonly List<InputTrigger> triggers = new();
	
	public readonly Action onTrigger;
	
	public InputAction(Action onTrigger)
	{
		this.onTrigger = onTrigger;
	}
}