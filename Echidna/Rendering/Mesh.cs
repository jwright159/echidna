namespace Echidna.Rendering;

public class Mesh : Component
{
	internal const int Dims = 3;
	
	public int NumVertices => positions.Length / Dims;
	
	internal bool isDirty;
	
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
	
	private float[] texCoords;
	public float[] TexCoords
	{
		get => texCoords;
		set
		{
			texCoords = value;
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
	
	internal int vertexBufferObject;
	internal int elementBufferObject;
	internal int vertexArrayObject;
	
	internal readonly bool cullBackFaces;
	
	internal bool hasBeenDisposed;
	
	public Mesh(float[] positions, float[] texCoords, float[] colors, uint[] indices, bool cullBackFaces = true)
	{
		this.positions = positions;
		this.texCoords = texCoords;
		this.colors = colors;
		this.indices = indices;
		
		this.cullBackFaces = cullBackFaces;
		
		data = Array.Empty<float>();
		isDirty = true;
	}
	
	~Mesh()
	{
		if (!hasBeenDisposed)
			Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
	}
}