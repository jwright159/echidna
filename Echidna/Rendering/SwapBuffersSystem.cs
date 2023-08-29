namespace Echidna.Rendering;

public class SwapBuffersSystem : System
{
	public SwapBuffersSystem() : base(typeof(Window)) { }
	
	public override void OnDraw(float deltaTime)
	{
		foreach (Entity entity in Entities)
			entity.GetComponent<Window>().window.SwapBuffers();
	}
}