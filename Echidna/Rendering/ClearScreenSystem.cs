using OpenTK.Graphics.OpenGL4;

namespace Echidna.Rendering;

public class ClearScreenSystem : System
{
	public ClearScreenSystem() : base(typeof(Window)) { }
	
	public override void OnInitialize()
	{
		foreach (Entity entity in Entities)
			Clear(entity.GetComponent<Window>());
	}
	
	private static void Clear(Window window)
	{
		GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
		GL.Clear(ClearBufferMask.ColorBufferBit);
		window.window.SwapBuffers();
		GL.Clear(ClearBufferMask.ColorBufferBit);
	}
}