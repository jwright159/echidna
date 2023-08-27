using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Echidna;

public class Mesh
{
	private const int Dims = 3;
	
	private readonly float[] positions =
	{
		0.5f, -0.5f, 0.0f,
		-0.5f, -0.5f, 0.0f,
		0.0f, 0.5f, 0.0f,
	};
	
	private readonly float[] colors =
	{
		1.0f, 0.0f, 0.0f,
		0.0f, 1.0f, 0.0f,
		0.0f, 0.0f, 1.0f,
	};
	
	private readonly uint[] indices =
	{
		0, 1, 2,
	};
	
	private readonly float[] data;
	
	private readonly Shader shader;
	
	private int vertexBufferObject;
	private int elementBufferObject;
	private int vertexArrayObject;
	
	public Mesh(Shader shader, Vector3 position)
	{
		this.shader = shader;
		
		int numVertices = positions.Length / Dims;
		for (int i = 0; i < numVertices; i++)
		for (int x = 0; x < Dims; x++)
			positions[i * Dims + x] += position[x];
		
		float[][] datasets = { positions, colors };
		string[] attributes = { "aPosition", "aColor" };
		
		data = new float[datasets.Sum(data => data.Length)];
		
		for (int i = 0; i < numVertices; i++)
		for (int dataset = 0; dataset < datasets.Length; dataset++)
		for (int x = 0; x < Dims; x++)
			data[i * datasets.Length * Dims + dataset * Dims + x] = datasets[dataset][i * Dims + x];
		
		vertexBufferObject = GL.GenBuffer();
		GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
		GL.BufferData(BufferTarget.ArrayBuffer, data.Length * sizeof(float), data, BufferUsageHint.StaticDraw);
		
		vertexArrayObject = GL.GenVertexArray();
		GL.BindVertexArray(vertexArrayObject);
		for (int attribute = 0; attribute < attributes.Length; attribute++)
		{
			int attribLocation = shader.GetAttribLocation(attributes[attribute]);
			GL.VertexAttribPointer(attribLocation, 3, VertexAttribPointerType.Float, false, attributes.Length * Dims * sizeof(float), attribute * Dims * sizeof(float));
			GL.EnableVertexAttribArray(attribLocation);
		}
		
		elementBufferObject = GL.GenBuffer();
		GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
		GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
	}
	
	public void Draw()
	{
		shader.SetMatrix4("model", Matrix4.Identity);
		
		GL.BindVertexArray(vertexArrayObject);
		GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
	}
}