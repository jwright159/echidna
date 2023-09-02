using Vector3System = System.Numerics.Vector3;
using Vector3OpenTK = OpenTK.Mathematics.Vector3;

namespace Echidna.Mathematics;

public struct Vector3
{
	public float X { get; set; }
	public float Y { get; set; }
	public float Z { get; set; }
	
	public Vector3(float x, float y, float z)
	{
		X = x;
		Y = y;
		Z = z;
	}
	
	public static Vector3 Right => new(1, 0, 0);
	public static Vector3 East => new(1, 0, 0);
	public static Vector3 Left => new(-1, 0, 0);
	public static Vector3 West => new(-1, 0, 0);
	public static Vector3 Forward => new(0, 1, 0);
	public static Vector3 North => new(0, 1, 0);
	public static Vector3 Back => new(0, -1, 0);
	public static Vector3 South => new(0, -1, 0);
	public static Vector3 Up => new(0, 0, 1);
	public static Vector3 Down => new(0, 0, -1);
	public static Vector3 One => new(1, 1, 1);
	public static Vector3 Zero => new(0, 0, 0);
	
	public static Vector3 operator+(Vector3 a, Vector3 b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
	public static Vector3 operator*(Vector3 vector, float scalar) => new(vector.X * scalar, vector.Y * scalar, vector.Z * scalar);
	public static Vector3 operator*(float scalar, Vector3 vector) => new(vector.X * scalar, vector.Y * scalar, vector.Z * scalar);
	
	public static implicit operator Vector3OpenTK(Vector3 vector) => new(vector.X, vector.Y, vector.Z);
	public static implicit operator Vector3(Vector3OpenTK vector) => new(vector.X, vector.Y, vector.Z);
	public static implicit operator Vector3System(Vector3 vector) => new(vector.X, vector.Y, vector.Z);
	public static implicit operator Vector3(Vector3System vector) => new(vector.X, vector.Y, vector.Z);
	public static implicit operator Vector3((float X, float Y, float Z) vector) => new(vector.X, vector.Y, vector.Z);
}