namespace Echidna;

public class Mesh : Component
{
	internal readonly int dims = 3;
	
	public readonly string[] attributes = { "aPosition", "aColor" };
	
	public int NumVertices => positions.Length / dims;
	
	internal bool isDirty = true;
	
	private float[] positions;
	public float[] Positions
	{
		get => positions;
		set
		{
			positions = value;
			isDirty = true;
		}
	}
	
	private float[] colors;
	public float[] Colors
	{
		get => colors;
		set
		{
			colors = value;
			isDirty = true;
		}
	}
	
	private uint[] indices;
	public uint[] Indices
	{
		get => indices;
		set
		{
			indices = value;
			isDirty = true;
		}
	}
	
	internal float[] data;
	
	internal readonly Shader shader;
	
	internal int vertexBufferObject;
	internal int elementBufferObject;
	internal int vertexArrayObject;
	
	internal bool initialized;
	internal bool disposed;
	
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
		
		indices = new uint[]
		{
			0, 1, 2,
		};
	}
	
	~Mesh()
	{
		if (!disposed)
			Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
	}
}