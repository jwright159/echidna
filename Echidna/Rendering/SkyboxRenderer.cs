namespace Echidna.Rendering;

public class SkyboxRenderer : Component
{
	public readonly Mesh mesh;
	public readonly Shader shader;
	public readonly CubeMap cubeMap;
	
	public SkyboxRenderer(Mesh mesh, Shader shader, CubeMap cubeMap)
	{
		this.mesh = mesh;
		this.shader = shader;
		this.cubeMap = cubeMap;
	}
}