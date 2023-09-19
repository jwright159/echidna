using Echidna.Core;

namespace Echidna.Rendering.Texture;

public class SkyboxRenderer : WorldComponent
{
	public readonly Mesh.Mesh Mesh;
	public readonly Shader.Shader Shader;
	public readonly CubeMap CubeMap;
	
	public SkyboxRenderer(Mesh.Mesh mesh, Shader.Shader shader, CubeMap cubeMap)
	{
		Mesh = mesh;
		Shader = shader;
		CubeMap = cubeMap;
	}
}