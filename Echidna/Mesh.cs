using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Echidna;

public class Mesh : IDisposable
{
	private const int Dims = 3;
	
	private readonly string[] attributes = { "aPosition", "aColor" };
	
	public int NumVertices => positions.Length / Dims;
	
	private float[] positions;
	public float[] Positions
	{
		get => positions;
		set
		{
			positions = value;
			RegenerateData();
			if (initialized) BindData();
		}
	}
	
	private readonly float[] colors;
	
	private uint[] indices;
	public uint[] Indices
	{
		get => indices;
		set
		{
			indices = value;
			if (initialized) BindIndices();
		}
	}
	
	private float[] data;
	
	private readonly Shader shader;
	
	private int vertexBufferObject;
	private int elementBufferObject;
	private int vertexArrayObject;
	
	private bool initialized;
	private bool disposed;
	
	public Mesh(Shader shader, Vector3 position)
	{
		this.shader = shader;
		
		positions = new[]
		{
			0.5f, -0.5f, 0.0f,
			-0.5f, -0.5f, 0.0f,
			0.0f, 0.5f, 0.0f,
		};
		for (int i = 0; i < NumVertices; i++)
		for (int x = 0; x < Dims; x++)
			positions[i * Dims + x] += position[x];
		
		colors = new[]
		{
			1.0f, 0.0f, 0.0f,
			0.0f, 1.0f, 0.0f,
			0.0f, 0.0f, 1.0f,
		};
		
		data = Array.Empty<float>();
		RegenerateData();
		
		indices = new uint[]
		{
			0, 1, 2,
		};
	}
	
	public void Initialize()
	{
		if (initialized) return;
		initialized = true;
		
		vertexBufferObject = GL.GenBuffer();
		BindData();
		
		vertexArrayObject = GL.GenVertexArray();
		GL.BindVertexArray(vertexArrayObject);
		for (int attribute = 0; attribute < attributes.Length; attribute++)
		{
			int attribLocation = shader.GetAttribLocation(attributes[attribute]);
			GL.VertexAttribPointer(attribLocation, 3, VertexAttribPointerType.Float, false, attributes.Length * Dims * sizeof(float), attribute * Dims * sizeof(float));
			GL.EnableVertexAttribArray(attribLocation);
		}
		
		elementBufferObject = GL.GenBuffer();
		BindIndices();
	}
	
	private void RegenerateData()
	{
		float[][] datasets = { positions, colors };
		data = new float[datasets.Sum(data => data.Length)];
		
		for (int i = 0; i < NumVertices; i++)
		for (int dataset = 0; dataset < datasets.Length; dataset++)
		for (int x = 0; x < Dims; x++)
			data[i * datasets.Length * Dims + dataset * Dims + x] = datasets[dataset][i * Dims + x];
	}
	
	private void BindData()
	{
		GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
		GL.BufferData(BufferTarget.ArrayBuffer, data.Length * sizeof(float), data, BufferUsageHint.StaticDraw);
	}
	
	private void BindIndices()
	{
		GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
		GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
	}
	
	public void Draw()
	{
		if (!initialized) throw new InvalidOperationException("Object wasn't initialized");
		
		shader.SetMatrix4("model", Matrix4.Identity);
		
		GL.BindVertexArray(vertexArrayObject);
		GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
	}
	
	public void Dispose()
	{
		if (disposed) return;
		disposed = true;
		if (!initialized) return;
		
		GL.DeleteBuffer(vertexBufferObject);
		GL.DeleteVertexArray(vertexArrayObject);
		GL.DeleteBuffer(elementBufferObject);
	}
	
	~Mesh()
	{
		if (!disposed)
			Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
	}
}