using OpenTK.Graphics.OpenGL4;

namespace Echidna.Rendering;

public class SwapBuffersSystem : System
{
	public SwapBuffersSystem() : base(typeof(Window)) { }
	
	[DrawEach]
	private static void SwapBuffers(float deltaTime, Window window)
	{
		window.window.SwapBuffers();
		//CopyBuffer(window);
	}
	
	/** Important if you're not clearing the screen */
	private static void CopyBuffer(Window window)
	{
		GL.ReadBuffer(ReadBufferMode.Front);
		GL.DrawBuffer(DrawBufferMode.Back);
		GL.BlitFramebuffer(
			0, 0, window.window.Size.X, window.window.Size.Y,
			0, 0, window.window.Size.X, window.window.Size.Y,
			ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit,
			BlitFramebufferFilter.Nearest);
	}
}