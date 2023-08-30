using OpenTK.Graphics.OpenGL4;

namespace Echidna.Rendering;

public class ClearScreenSystem : System
{
	public ClearScreenSystem() : base(typeof(Window)) { }
	
	[InitializeEach]
	private static void SetupClear(Window window)
	{
		GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
		GL.Enable(EnableCap.DepthTest);
	}
	
	[DrawEach]
	private static void Clear(Window window)
	{
		GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
	}
}