namespace Echidna.Rendering;

public class SkyboxRenderer : Component
{
	public readonly Mesh Mesh;
	public readonly Shader Shader;
	public readonly CubeMap CubeMap;
	
	public SkyboxRenderer(Mesh mesh, Shader shader, CubeMap cubeMap)
	{
		Mesh = mesh;
		Shader = shader;
		CubeMap = cubeMap;
	}
}