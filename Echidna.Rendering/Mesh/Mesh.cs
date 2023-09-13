using Echidna.Core;

namespace Echidna.Rendering.Mesh;

public class Mesh : Component
{
	internal const int Dims = 3;
	
	public int NumVertices => positions.Length / Dims;
	
	internal bool IsDirty;
	
	private float[] positions;
	public float[] Positions
	{
		get => positions;
		set
		{
			positions = value;
			IsDirty = true;
		}
	}
	
	private float[] texCoords;
	public float[] TexCoords
	{
		get => texCoords;
		set
		{
			texCoords = value;
			IsDirty = true;
		}
	}
	
	private float[] colors;
	public float[] Colors
	{
		get => colors;
		set
		{
			colors = value;
			IsDirty = true;
		}
	}
	
	private uint[] indices;
	public uint[] Indices
	{
		get => indices;
		set
		{
			indices = value;
			IsDirty = true;
		}
	}
	
	internal float[] Data;
	
	internal int VertexBufferObject;
	internal int ElementBufferObject;
	internal int VertexArrayObject;
	
	internal readonly bool CullBackFaces;
	
	internal bool HasBeenDisposed;
	
	public Mesh(float[] positions, float[] texCoords, float[] colors, uint[] indices, bool cullBackFaces = true)
	{
		this.positions = positions;
		this.texCoords = texCoords;
		this.colors = colors;
		this.indices = indices;
		
		CullBackFaces = cullBackFaces;
		
		Data = Array.Empty<float>();
		IsDirty = true;
	}
	
	~Mesh()
	{
		if (!HasBeenDisposed)
			Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
	}
}