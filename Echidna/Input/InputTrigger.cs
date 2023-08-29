namespace Echidna.Input;

public interface InputTrigger
{
	public bool IsTriggeredBy(object type);
	public object FactorIn(object type, float value);
}

public interface InputTrigger<out T> : InputTrigger where T : notnull
{
	public new T FactorIn(object type, float value);
	
	object InputTrigger.FactorIn(object type, float value) => FactorIn(type, value);
}