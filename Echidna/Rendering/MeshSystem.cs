using Echidna.Core;
using OpenTK.Graphics.OpenGL4;

namespace Echidna.Rendering;

public class MeshSystem : System<Mesh>
{
	protected override void OnInitializeEach(Mesh mesh)
	{
		mesh.VertexBufferObject = GL.GenBuffer();
		GL.BindBuffer(BufferTarget.ArrayBuffer, mesh.VertexBufferObject);
		
		mesh.VertexArrayObject = GL.GenVertexArray();
		GL.BindVertexArray(mesh.VertexArrayObject);
		
		int[] widths = { 3, 2, 3 };
		int stride = widths.Sum();
		for (int attribute = 0, offset = 0; attribute < widths.Length; offset += widths[attribute], attribute++)
		{
			GL.EnableVertexAttribArray(attribute);
			GL.VertexAttribPointer(attribute, widths[attribute], VertexAttribPointerType.Float, false, stride * sizeof(float), offset * sizeof(float));
		}
		
		mesh.ElementBufferObject = GL.GenBuffer();
		GL.BindBuffer(BufferTarget.ElementArrayBuffer, mesh.ElementBufferObject);
	}
	
	protected override void OnDrawEach(Mesh mesh)
	{
		if (mesh.IsDirty)
			CleanMesh(mesh);
	}
	
	private static void CleanMesh(Mesh mesh)
	{
		mesh.IsDirty = false;
		RegenerateData(mesh);
		BindData(mesh.VertexBufferObject, mesh.Data);
		BindIndices(mesh.ElementBufferObject, mesh.Indices);
	}
	
	private static void RegenerateData(Mesh mesh)
	{
		float[][] datasets = { mesh.Positions, mesh.TexCoords, mesh.Colors };
		int[] widths = { 3, 2, 3 };
		int stride = widths.Sum();
		mesh.Data = new float[datasets.Sum(data => data.Length)];
		
		for (int i = 0; i < mesh.NumVertices; i++)
		for (int dataset = 0, offset = 0; dataset < datasets.Length; offset += widths[dataset], dataset++)
		for (int x = 0; x < widths[dataset]; x++)
			mesh.Data[i * stride + offset + x] = datasets[dataset][i * widths[dataset] + x];
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
		mesh.HasBeenDisposed = true;
		GL.DeleteBuffer(mesh.VertexBufferObject);
		GL.DeleteVertexArray(mesh.VertexArrayObject);
		GL.DeleteBuffer(mesh.ElementBufferObject);
	}
}