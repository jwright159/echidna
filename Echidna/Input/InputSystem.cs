using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Echidna.Input;

public class InputSystem : System
{
	public InputSystem() : base(typeof(InputGroup)) { }
	
	private static void Handle(InputGroup input, object type, float value)
	{
		if (input.HasActionFor(type))
			foreach (InputAction action in input[type])
			foreach (InputTrigger trigger in action[type])
				action.Action(trigger.FactorIn(type, value));
	}
	
	[MouseMoveEach]
	private static void OnMouseMove(Vector2 position, Vector2 delta, InputGroup input)
	{
		Handle(input, MouseAxis.X, position.X);
		Handle(input, MouseAxis.Y, position.Y);
		Handle(input, MouseAxis.DeltaX, delta.X);
		Handle(input, MouseAxis.DeltaY, delta.Y);
	}
	
	[KeyDownEach]
	private static void OnKeyDown(Keys key, InputGroup input) => OnKey(key, 1, input);
	
	[KeyUpEach]
	private static void OnKeyUp(Keys key, InputGroup input) => OnKey(key, 0, input);
	
	private static void OnKey(Keys key, float value, InputGroup input)
	{
		Handle(input, key, value);
	}
}