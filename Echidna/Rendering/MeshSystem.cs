using OpenTK.Graphics.OpenGL4;

namespace Echidna.Rendering;

public class MeshSystem : System
{
	public MeshSystem() : base(typeof(Mesh)) { }
	
	[InitializeEach]
	private static void Initialize(Mesh mesh)
	{
		mesh.vertexBufferObject = GL.GenBuffer();
		GL.BindBuffer(BufferTarget.ArrayBuffer, mesh.vertexBufferObject);
		
		mesh.vertexArrayObject = GL.GenVertexArray();
		GL.BindVertexArray(mesh.vertexArrayObject);
		for (int attribute = 0; attribute < mesh.attributes.Length; attribute++)
		{
			GL.VertexAttribPointer(attribute, 3, VertexAttribPointerType.Float, false, mesh.attributes.Length * Mesh.Dims * sizeof(float), attribute * Mesh.Dims * sizeof(float));
			GL.EnableVertexAttribArray(attribute);
		}
		
		mesh.elementBufferObject = GL.GenBuffer();
		GL.BindBuffer(BufferTarget.ElementArrayBuffer, mesh.elementBufferObject);
	}
	
	[DrawEach]
	private static void Draw(Mesh mesh)
	{
		if (mesh.isDirty)
			CleanMesh(mesh);
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
		for (int x = 0; x < Mesh.Dims; x++)
			mesh.data[i * datasets.Length * Mesh.Dims + dataset * Mesh.Dims + x] = datasets[dataset][i * Mesh.Dims + x];
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
	
	[DisposeEach]
	private static void Dispose(Mesh mesh)
	{
		mesh.hasBeenDisposed = true;
		GL.DeleteBuffer(mesh.vertexBufferObject);
		GL.DeleteVertexArray(mesh.vertexArrayObject);
		GL.DeleteBuffer(mesh.elementBufferObject);
	}
}