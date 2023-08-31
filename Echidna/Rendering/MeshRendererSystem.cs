using Echidna.Hierarchy;
using OpenTK.Graphics.OpenGL4;

namespace Echidna.Rendering;

public class MeshRendererSystem : System
{
	public MeshRendererSystem() : base(typeof(Transform), typeof(MeshRenderer)) { }
	
	[DrawAll]
	private static void Draw(List<Entity> entities)
	{
		Mesh? currentMesh = null;
		Shader? currentShader = null;
		
		foreach (Entity entity in entities)
		{
			Transform transform = entity.GetComponent<Transform>();
			MeshRenderer meshRenderer = entity.GetComponent<MeshRenderer>();
			
			if (meshRenderer.shader != currentShader)
			{
				currentShader = meshRenderer.shader;
				meshRenderer.shader.Bind();
			}
			
			if (meshRenderer.mesh != currentMesh)
			{
				currentMesh = meshRenderer.mesh;
				GL.BindVertexArray(meshRenderer.mesh.vertexArrayObject);
			}
			
			meshRenderer.shader.SetMatrix4(0, transform.Transformation);
			GL.DrawElements(PrimitiveType.Triangles, meshRenderer.mesh.Indices.Length, DrawElementsType.UnsignedInt, 0);
		}
	}
}