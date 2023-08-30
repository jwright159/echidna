using OpenTK.Mathematics;

namespace Echidna.Input;

public class Axis2InputTrigger : InputTrigger<Vector2>
{
	private HashSet<object> inputTypes;
	
	private AxisInputTrigger x;
	private AxisInputTrigger y;
	
	private float currentXValue;
	private float currentYValue;
	
	private bool normalize;
	
	public Axis2InputTrigger(AxisInputTrigger x, AxisInputTrigger y, bool normalize = true)
	{
		this.x = x;
		this.y = y;
		this.normalize = normalize;
		
		inputTypes = x.InputTypes.Concat(y.InputTypes).ToHashSet();
	}
	
	public IEnumerable<object> InputTypes => inputTypes;
	
	public bool IsTriggeredBy(object type) => inputTypes.Contains(type);
	
	public Vector2 FactorIn(object type, float value)
	{
		if (x.IsTriggeredBy(type))
			currentXValue = x.FactorIn(type, value);
		if (y.IsTriggeredBy(type))
			currentYValue = y.FactorIn(type, value);
		
		Vector2 currentValue = (currentXValue, currentYValue);
		if (normalize && currentValue.Length > 1) currentValue = currentValue.Normalized();
		return currentValue;
	}
}