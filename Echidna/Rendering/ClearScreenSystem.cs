using OpenTK.Graphics.OpenGL4;

namespace Echidna.Rendering;

public class ClearScreenSystem : System
{
	public ClearScreenSystem() : base(typeof(Window)) { }
	
	public override void OnInitialize()
	{
		foreach (Entity entity in Entities)
			SetupClear(entity.GetComponent<Window>());
	}
	
	public override void OnDraw(float deltaTime)
	{
		foreach (Entity entity in Entities)
			Clear(entity.GetComponent<Window>());
	}
	
	private static void SetupClear(Window window)
	{
		GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
		GL.Enable(EnableCap.DepthTest);
	}
	
	private static void Clear(Window window)
	{
		GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
	}
}