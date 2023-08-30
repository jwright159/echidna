using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Echidna.Input;

public class InputSystem : System
{
	public InputSystem() : base(typeof(InputGroup)) { }
	
	private static void Handle(InputAction action, object type, float value)
	{
		foreach (InputTrigger trigger in action.triggers.Where(trigger => trigger.IsTriggeredBy(type)))
			action.Action(trigger.FactorIn(type, value));
	}
	
	[MouseMoveEach]
	private static void OnMouseMove(Vector2 position, Vector2 delta, InputGroup input)
	{
		foreach (InputAction action in input.actions)
		{
			Handle(action, MouseAxis.X, position.X);
			Handle(action, MouseAxis.Y, position.Y);
			Handle(action, MouseAxis.DeltaX, delta.X);
			Handle(action, MouseAxis.DeltaY, delta.Y);
		}
	}
	
	[KeyDownEach]
	private static void OnKeyDown(Keys key, InputGroup input) => OnKey(key, 1, input);
	
	[KeyUpEach]
	private static void OnKeyUp(Keys key, InputGroup input) => OnKey(key, 0, input);
	
	private static void OnKey(Keys key, float value, InputGroup input)
	{
		foreach (InputAction action in input.actions)
			Handle(action, key, value);
	}
}