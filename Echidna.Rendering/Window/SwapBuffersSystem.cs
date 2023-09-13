using Echidna.Core;
using OpenTK.Graphics.OpenGL4;

namespace Echidna.Rendering.Window;

public class SwapBuffersSystem : System<Window>
{
	protected override void OnDrawEach(Window window)
	{
		window.GameWindow.SwapBuffers();
		//CopyBuffer(window);
	}
	
	/// <summary>
	/// Important if you're not clearing the screen
	/// </summary>
	private static void CopyBuffer(Window window)
	{
		GL.ReadBuffer(ReadBufferMode.Front);
		GL.DrawBuffer(DrawBufferMode.Back);
		GL.BlitFramebuffer(
			0, 0, window.GameWindow.Size.X, window.GameWindow.Size.Y,
			0, 0, window.GameWindow.Size.X, window.GameWindow.Size.Y,
			ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit,
			BlitFramebufferFilter.Nearest);
	}
}