using OpenTK.Windowing.Desktop;

namespace Echidna;

public class Window : Component
{
	public readonly GameWindow window;
	
	public Window(GameWindow window)
	{
		this.window = window;
	}
}