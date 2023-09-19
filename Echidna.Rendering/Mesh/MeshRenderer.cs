using Echidna.Core;

namespace Echidna.Rendering.Mesh;

public class MeshRenderer : EntityComponent
{
	public readonly Mesh Mesh;
	public readonly Shader.Shader Shader;
	public readonly Texture.Texture? Texture;
	
	public MeshRenderer(Mesh mesh, Shader.Shader shader, Texture.Texture? texture)
	{
		Mesh = mesh;
		Shader = shader;
		Texture = texture;
	}
}