using Echidna.Hierarchy;
using OpenTK.Graphics.OpenGL4;

namespace Echidna.Rendering;

public class MeshRendererSystem : System<Transform, MeshRenderer>
{
	protected override void OnInitialize(IEnumerable<(Transform, MeshRenderer)> componentSets)
	{
		GL.FrontFace(FrontFaceDirection.Cw);
	}
	
	protected override void OnDraw(IEnumerable<(Transform, MeshRenderer)> componentSets)
	{
		Mesh? currentMesh = null;
		Shader? currentShader = null;
		Texture? currentTexture = null;
		
		bool backfaceCullingEnabled = true;
		GL.Enable(EnableCap.CullFace);
		
		foreach ((Transform transform, MeshRenderer meshRenderer) in componentSets)
		{
			if (meshRenderer.shader != currentShader)
			{
				currentShader = meshRenderer.shader;
				meshRenderer.shader.Bind();
			}
			
			if (meshRenderer.texture != null && meshRenderer.texture != currentTexture)
			{
				currentTexture = meshRenderer.texture;
				meshRenderer.texture.Bind();
				meshRenderer.shader.SetInt("texture0", 0);
			}
			
			if (meshRenderer.mesh != currentMesh)
			{
				currentMesh = meshRenderer.mesh;
				meshRenderer.mesh.Bind();
			}
			
			if (backfaceCullingEnabled && !meshRenderer.mesh.cullBackFaces)
			{
				backfaceCullingEnabled = false;
				GL.Disable(EnableCap.CullFace);
			}
			else if (!backfaceCullingEnabled && meshRenderer.mesh.cullBackFaces)
			{
				backfaceCullingEnabled = true;
				GL.Enable(EnableCap.CullFace);
			}
			
			meshRenderer.shader.SetMatrix4(0, transform.Transformation);
			meshRenderer.mesh.Draw();
		}
	}
}