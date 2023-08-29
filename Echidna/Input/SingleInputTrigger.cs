namespace Echidna.Input;

public class SingleInputTrigger : InputTrigger<float>
{
	private object inputType;
	
	public SingleInputTrigger(object inputType)
	{
		this.inputType = inputType;
	}
	
	public bool IsTriggeredBy(object type) => type.Equals(inputType);
	
	public float FactorIn(object type, float value) => value;
}