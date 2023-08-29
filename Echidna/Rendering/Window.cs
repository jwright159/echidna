using OpenTK.Windowing.Desktop;

namespace Echidna.Rendering;

public class Window : Component
{
	public readonly GameWindow window;
	
	public Window(GameWindow window)
	{
		this.window = window;
	}
}