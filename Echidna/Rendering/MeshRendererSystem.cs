using Echidna.Core;
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
			if (meshRenderer.Shader != currentShader)
			{
				currentShader = meshRenderer.Shader;
				meshRenderer.Shader.Bind();
			}
			
			if (meshRenderer.Texture != null && meshRenderer.Texture != currentTexture)
			{
				currentTexture = meshRenderer.Texture;
				meshRenderer.Texture.Bind();
				meshRenderer.Shader.SetInt("texture0", 0);
			}
			
			if (meshRenderer.Mesh != currentMesh)
			{
				currentMesh = meshRenderer.Mesh;
				meshRenderer.Mesh.Bind();
			}
			
			if (backfaceCullingEnabled && !meshRenderer.Mesh.CullBackFaces)
			{
				backfaceCullingEnabled = false;
				GL.Disable(EnableCap.CullFace);
			}
			else if (!backfaceCullingEnabled && meshRenderer.Mesh.CullBackFaces)
			{
				backfaceCullingEnabled = true;
				GL.Enable(EnableCap.CullFace);
			}
			
			meshRenderer.Shader.SetMatrix4(0, transform.Transformation);
			meshRenderer.Mesh.Draw();
		}
	}
}