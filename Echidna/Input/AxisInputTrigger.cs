namespace Echidna.Input;

public class AxisInputTrigger : InputTrigger<float>
{
	private SingleInputTrigger positive;
	private SingleInputTrigger negative;
	
	private float currentPositiveValue;
	private float currentNegativeValue;
	
	public AxisInputTrigger(SingleInputTrigger positive, SingleInputTrigger negative)
	{
		this.positive = positive;
		this.negative = negative;
	}
	
	public bool IsTriggeredBy(object type) => positive.IsTriggeredBy(type) || negative.IsTriggeredBy(type);
	
	public float FactorIn(object type, float value)
	{
		if (positive.IsTriggeredBy(type))
			currentPositiveValue = positive.FactorIn(type, value);
		if (negative.IsTriggeredBy(type))
			currentNegativeValue = negative.FactorIn(type, value);
		return currentPositiveValue - currentNegativeValue;
	}
}