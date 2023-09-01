using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Echidna.Input;

public class InputSystem : System<InputGroup>
{
	private static void Handle(InputGroup input, object type, float value)
	{
		if (input.HasActionFor(type))
			foreach (InputAction action in input[type])
			foreach (InputTrigger trigger in action[type])
				action.Action(trigger.FactorIn(type, value));
	}
	
	protected override void OnMouseMoveEach(Vector2 position, Vector2 delta, InputGroup input)
	{
		Handle(input, MouseAxis.X, position.X);
		Handle(input, MouseAxis.Y, position.Y);
		Handle(input, MouseAxis.DeltaX, delta.X);
		Handle(input, MouseAxis.DeltaY, delta.Y);
	}
	
	protected override void OnKeyDownEach(Keys key, InputGroup input) => OnKey(key, 1, input);
	
	protected override void OnKeyUpEach(Keys key, InputGroup input) => OnKey(key, 0, input);
	
	private static void OnKey(Keys key, float value, InputGroup input)
	{
		Handle(input, key, value);
	}
}