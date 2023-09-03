using Echidna.Core;
using OpenTK.Graphics.OpenGL4;

namespace Echidna.Rendering;

public class SkyboxRendererSystem : System<SkyboxRenderer>
{
	protected override void OnDraw(IEnumerable<SkyboxRenderer> skyboxRenderers)
	{
		Mesh? currentMesh = null;
		Shader? currentShader = null;
		CubeMap? currentCubeMap = null;
		
		GL.Disable(EnableCap.CullFace);
		GL.DepthFunc(DepthFunction.Lequal);
		
		foreach (SkyboxRenderer skyboxRenderer in skyboxRenderers)
		{
			if (skyboxRenderer.Shader != currentShader)
			{
				currentShader = skyboxRenderer.Shader;
				skyboxRenderer.Shader.Bind();
			}
			
			if (skyboxRenderer.CubeMap != currentCubeMap)
			{
				currentCubeMap = skyboxRenderer.CubeMap;
				skyboxRenderer.CubeMap.Bind();
			}
			
			if (skyboxRenderer.Mesh != currentMesh)
			{
				currentMesh = skyboxRenderer.Mesh;
				skyboxRenderer.Mesh.Bind();
			}
			
			skyboxRenderer.Mesh.Draw();
		}
		
		GL.DepthFunc(DepthFunction.Less);
	}
}