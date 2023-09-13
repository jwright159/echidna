using Echidna.Core;
using OpenTK.Graphics.OpenGL4;

namespace Echidna.Rendering.Window;

public class ClearScreenSystem : System<Window>
{
	protected override void OnInitializeEach(Window window)
	{
		GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
		GL.Enable(EnableCap.DepthTest);
	}
	
	protected override void OnDrawEach(Window window)
	{
		GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
	}
}