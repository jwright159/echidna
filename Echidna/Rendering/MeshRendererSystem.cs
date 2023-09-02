using Echidna.Hierarchy;
using OpenTK.Graphics.OpenGL4;

namespace Echidna.Rendering;

public class MeshRendererSystem : System<Transform, MeshRenderer>
{
	private Mesh? currentMesh;
	private Shader? currentShader;
	
	protected override void OnDraw(IEnumerable<(Transform, MeshRenderer)> componentSets)
	{
		currentMesh = null;
		currentShader = null;
	}
	
	protected override void OnDrawEach(Transform transform, MeshRenderer meshRenderer)
	{
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