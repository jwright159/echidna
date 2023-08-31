namespace Echidna.Rendering;

public class MeshRenderer : Component
{
	public readonly Mesh mesh;
	public readonly Shader shader;
	
	public MeshRenderer(Mesh mesh, Shader shader)
	{
		this.mesh = mesh;
		this.shader = shader;
	}
}