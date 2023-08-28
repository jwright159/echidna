using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Echidna;

public class Mesh : Component
{
	private int Dims = 3;
	
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
	
	public Mesh(Shader shader)
	{
		this.shader = shader;
		
		positions = new[]
		{
			0.5f, -0.5f, 0.0f,
			-0.5f, -0.5f, 0.0f,
			0.0f, 0.5f, 0.0f,
		};
		
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
	
	~Mesh()
	{
		if (!disposed)
			Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
	}
}