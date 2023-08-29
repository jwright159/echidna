using OpenTK.Mathematics;

namespace Echidna.Input;

public class Axis3InputTrigger : InputTrigger<Vector3>
{
	private AxisInputTrigger x;
	private AxisInputTrigger y;
	private AxisInputTrigger z;
	
	private float currentXValue;
	private float currentYValue;
	private float currentZValue;
	
	private bool normalize;
	
	public Axis3InputTrigger(AxisInputTrigger x, AxisInputTrigger y, AxisInputTrigger z, bool normalize = true)
	{
		this.x = x;
		this.y = y;
		this.z = z;
		this.normalize = normalize;
	}
	
	public bool IsTriggeredBy(object type) => x.IsTriggeredBy(type) || y.IsTriggeredBy(type) || z.IsTriggeredBy(type);
	
	public Vector3 FactorIn(object type, float value)
	{
		if (x.IsTriggeredBy(type))
			currentXValue = x.FactorIn(type, value);
		if (y.IsTriggeredBy(type))
			currentYValue = y.FactorIn(type, value);
		if (z.IsTriggeredBy(type))
			currentZValue = z.FactorIn(type, value);
		
		Vector3 currentValue = (currentXValue, currentYValue, currentZValue);
		if (normalize && currentValue.Length > 1) currentValue = currentValue.Normalized();
		return currentValue;
	}
}