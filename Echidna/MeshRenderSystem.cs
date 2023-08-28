using OpenTK.Graphics.OpenGL4;

namespace Echidna;

public class MeshRenderSystem : System
{
	public MeshRenderSystem() : base(typeof(Transform), typeof(Mesh))
	{
		
	}
	
	public override void OnInitialize()
	{
		foreach (Entity entity in Entities)
			Initialize(entity.GetComponent<Mesh>());
	}
	
	private void Initialize(Mesh mesh)
	{
		if (mesh.initialized) return;
		mesh.initialized = true;
			
		mesh.vertexBufferObject = GL.GenBuffer();
		BindData();
			
		mesh.vertexArrayObject = GL.GenVertexArray();
		GL.BindVertexArray(mesh.vertexArrayObject);
		for (int attribute = 0; attribute < mesh.attributes.Length; attribute++)
		{
			int attribLocation = mesh.shader.GetAttribLocation(mesh.attributes[attribute]);
			GL.VertexAttribPointer(attribLocation, 3, VertexAttribPointerType.Float, false, mesh.attributes.Length * mesh.Dims * sizeof(float), attribute * mesh.Dims * sizeof(float));
			GL.EnableVertexAttribArray(attribLocation);
		}
			
		mesh.elementBufferObject = GL.GenBuffer();
		BindIndices();
	}
	
	public override void OnDraw()
	{
		foreach (Entity entity in Entities)
			Draw(entity.GetComponent<Transform>(), entity.GetComponent<Mesh>());
	}
	
	private void Draw(Transform transform, Mesh mesh)
	{
		if (!mesh.initialized) throw new InvalidOperationException("Object wasn't initialized");
		
		mesh.shader.SetMatrix4("model", transform.Transformation);
		
		GL.BindVertexArray(mesh.vertexArrayObject);
		GL.DrawElements(PrimitiveType.Triangles, mesh.indices.Length, DrawElementsType.UnsignedInt, 0);
	}
	
	public override void OnDispose()
	{
		foreach (Entity entity in Entities)
			Dispose(entity.GetComponent<Mesh>());
	}
	
	private void Dispose(Mesh mesh)
	{
		if (mesh.disposed) return;
		mesh.disposed = true;
		if (!mesh.initialized) return;
			
		GL.DeleteBuffer(mesh.vertexBufferObject);
		GL.DeleteVertexArray(mesh.vertexArrayObject);
		GL.DeleteBuffer(mesh.elementBufferObject);
	}
}