namespace Echidna.Input;

public class AxisInputTrigger : InputTrigger<float>
{
	private HashSet<object> inputTypes;
	
	private SingleInputTrigger positive;
	private SingleInputTrigger negative;
	
	private float currentPositiveValue;
	private float currentNegativeValue;
	
	public AxisInputTrigger(SingleInputTrigger positive, SingleInputTrigger negative)
	{
		this.positive = positive;
		this.negative = negative;
		
		inputTypes = positive.InputTypes.Concat(negative.InputTypes).ToHashSet();
	}
	
	public IEnumerable<object> InputTypes => inputTypes;
	
	public bool IsTriggeredBy(object type) => inputTypes.Contains(type);
	
	public float FactorIn(object type, float value)
	{
		if (positive.IsTriggeredBy(type))
			currentPositiveValue = positive.FactorIn(type, value);
		if (negative.IsTriggeredBy(type))
			currentNegativeValue = negative.FactorIn(type, value);
		return currentPositiveValue - currentNegativeValue;
	}
}