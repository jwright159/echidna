namespace Echidna.Rendering;

public class MeshRenderer : Component
{
	public readonly Mesh Mesh;
	public readonly Shader Shader;
	public readonly Texture? Texture;
	
	public MeshRenderer(Mesh mesh, Shader shader, Texture? texture)
	{
		Mesh = mesh;
		Shader = shader;
		Texture = texture;
	}
}