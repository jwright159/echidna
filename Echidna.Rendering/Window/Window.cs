using Echidna.Core;
using OpenTK.Windowing.Desktop;

namespace Echidna.Rendering.Window;

public class Window : WorldComponent
{
	public readonly GameWindow GameWindow;
	
	public Window(GameWindow gameWindow)
	{
		GameWindow = gameWindow;
	}
}