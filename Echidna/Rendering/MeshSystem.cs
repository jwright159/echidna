using OpenTK.Graphics.OpenGL4;

namespace Echidna.Rendering;

public class MeshSystem : System<Mesh>
{
	protected override void OnInitializeEach(Mesh mesh)
	{
		mesh.vertexBufferObject = GL.GenBuffer();
		GL.BindBuffer(BufferTarget.ArrayBuffer, mesh.vertexBufferObject);
		
		mesh.vertexArrayObject = GL.GenVertexArray();
		GL.BindVertexArray(mesh.vertexArrayObject);
		
		int[] widths = { 3, 2, 3 };
		int stride = widths.Sum();
		for (int attribute = 0, offset = 0; attribute < widths.Length; offset += widths[attribute], attribute++)
		{
			GL.VertexAttribPointer(attribute, widths[attribute], VertexAttribPointerType.Float, false, stride * sizeof(float), offset * sizeof(float));
			GL.EnableVertexAttribArray(attribute);
		}
		
		mesh.elementBufferObject = GL.GenBuffer();
		GL.BindBuffer(BufferTarget.ElementArrayBuffer, mesh.elementBufferObject);
	}
	
	protected override void OnDrawEach(Mesh mesh)
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
		float[][] datasets = { mesh.Positions, mesh.TexCoords, mesh.Colors };
		int[] widths = { 3, 2, 3 };
		int stride = widths.Sum();
		mesh.data = new float[datasets.Sum(data => data.Length)];
		
		for (int i = 0; i < mesh.NumVertices; i++)
		for (int dataset = 0, offset = 0; dataset < datasets.Length; offset += widths[dataset], dataset++)
		for (int x = 0; x < widths[dataset]; x++)
			mesh.data[i * stride + offset + x] = datasets[dataset][i * widths[dataset] + x];
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
	
	protected override void OnDisposeEach(Mesh mesh)
	{
		mesh.hasBeenDisposed = true;
		GL.DeleteBuffer(mesh.vertexBufferObject);
		GL.DeleteVertexArray(mesh.vertexArrayObject);
		GL.DeleteBuffer(mesh.elementBufferObject);
	}
}