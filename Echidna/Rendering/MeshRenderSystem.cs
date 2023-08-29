using Echidna.Hierarchy;
using OpenTK.Graphics.OpenGL4;

namespace Echidna.Rendering;

public class MeshRenderSystem : System
{
	public MeshRenderSystem() : base(typeof(Transform), typeof(Mesh)) { }
	
	public override void OnInitialize()
	{
		foreach (Entity entity in Entities)
			Initialize(entity.GetComponent<Mesh>());
	}
	
	private static void Initialize(Mesh mesh)
	{
		mesh.vertexBufferObject = GL.GenBuffer();
		GL.BindBuffer(BufferTarget.ArrayBuffer, mesh.vertexBufferObject);
		
		mesh.vertexArrayObject = GL.GenVertexArray();
		GL.BindVertexArray(mesh.vertexArrayObject);
		for (int attribute = 0; attribute < mesh.attributes.Length; attribute++)
		{
			int attribLocation = mesh.shader.GetAttribLocation(mesh.attributes[attribute]);
			GL.VertexAttribPointer(attribLocation, 3, VertexAttribPointerType.Float, false, mesh.attributes.Length * mesh.dims * sizeof(float), attribute * mesh.dims * sizeof(float));
			GL.EnableVertexAttribArray(attribLocation);
		}
		
		mesh.elementBufferObject = GL.GenBuffer();
		GL.BindBuffer(BufferTarget.ElementArrayBuffer, mesh.elementBufferObject);
	}
	
	public override void OnDraw()
	{
		foreach (Entity entity in Entities)
			Draw(entity.GetComponent<Transform>(), entity.GetComponent<Mesh>());
	}
	
	private static void Draw(Transform transform, Mesh mesh)
	{
		if (mesh.isDirty)
			CleanMesh(mesh);
		
		mesh.shader.SetMatrix4("model", transform.Transformation);
		mesh.shader.Bind();
		
		GL.BindVertexArray(mesh.vertexArrayObject);
		GL.DrawElements(PrimitiveType.Triangles, mesh.Indices.Length, DrawElementsType.UnsignedInt, 0);
	}
	
	private static void CleanMesh(Mesh mesh)
	{
		mesh.isDirty = false;
		RegenerateData(mesh);
		BindData(mesh.vertexBufferObject, mesh.data);
		BindIndices(mesh.elementBufferObject, mesh.Indices);
	}
	
	private static void RegenerateData(Mesh mesh)
	{
		float[][] datasets = { mesh.Positions, mesh.Colors };
		mesh.data = new float[datasets.Sum(data => data.Length)];
		
		for (int i = 0; i < mesh.NumVertices; i++)
		for (int dataset = 0; dataset < datasets.Length; dataset++)
		for (int x = 0; x < mesh.dims; x++)
			mesh.data[i * datasets.Length * mesh.dims + dataset * mesh.dims + x] = datasets[dataset][i * mesh.dims + x];
	}
	
	private static void BindData(int vertexBufferObject, float[] data)
	{
		GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
		GL.BufferData(BufferTarget.ArrayBuffer, data.Length * sizeof(float), data, BufferUsageHint.StaticDraw);
	}
	
	private static void BindIndices(int elementBufferObject, uint[] indices)
	{
		GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
		GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
	}
	
	public override void OnDispose()
	{
		foreach (Entity entity in Entities)
			Dispose(entity.GetComponent<Mesh>());
	}
	
	private static void Dispose(Mesh mesh)
	{
		mesh.hasBeenDisposed = true;
		GL.DeleteBuffer(mesh.vertexBufferObject);
		GL.DeleteVertexArray(mesh.vertexArrayObject);
		GL.DeleteBuffer(mesh.elementBufferObject);
	}
}