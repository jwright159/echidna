namespace Echidna.Rendering;

public class MeshRenderer : Component
{
	public readonly Mesh mesh;
	public readonly Shader shader;
	public readonly Texture? texture;
	
	public MeshRenderer(Mesh mesh, Shader shader, Texture? texture)
	{
		this.mesh = mesh;
		this.shader = shader;
		this.texture = texture;
	}
}