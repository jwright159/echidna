using OpenTK.Graphics.OpenGL4;

namespace Echidna.Rendering;

public class SwapBuffersSystem : System
{
	public SwapBuffersSystem() : base(typeof(Window)) { }
	
	public override void OnDraw(float deltaTime)
	{
		foreach (Entity entity in Entities)
			SwapBuffers(entity.GetComponent<Window>());
	}
	
	private static void SwapBuffers(Window window)
	{
		window.window.SwapBuffers();
		GL.ReadBuffer(ReadBufferMode.Front);
		GL.DrawBuffer(DrawBufferMode.Back);
		GL.BlitFramebuffer(
			0, 0, window.window.Size.X, window.window.Size.Y,
			0, 0, window.window.Size.X, window.window.Size.Y,
			ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit,
			BlitFramebufferFilter.Nearest);
	}
}