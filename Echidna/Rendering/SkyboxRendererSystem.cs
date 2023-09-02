using OpenTK.Graphics.OpenGL4;

namespace Echidna.Rendering;

public class SkyboxRendererSystem : System<SkyboxRenderer>
{
	protected override void OnDraw(IEnumerable<SkyboxRenderer> skyboxRenderers)
	{
		Mesh? currentMesh = null;
		Shader? currentShader = null;
		CubeMap? currentCubeMap = null;
		
		GL.DepthFunc(DepthFunction.Lequal);
		
		foreach (SkyboxRenderer skyboxRenderer in skyboxRenderers)
		{
			if (skyboxRenderer.shader != currentShader)
			{
				currentShader = skyboxRenderer.shader;
				skyboxRenderer.shader.Bind();
			}
			
			if (skyboxRenderer.cubeMap != currentCubeMap)
			{
				currentCubeMap = skyboxRenderer.cubeMap;
				skyboxRenderer.cubeMap.Bind();
			}
			
			if (skyboxRenderer.mesh != currentMesh)
			{
				currentMesh = skyboxRenderer.mesh;
				skyboxRenderer.mesh.Bind();
			}
			
			skyboxRenderer.mesh.Draw();
		}
		
		GL.DepthFunc(DepthFunction.Less);
	}
}