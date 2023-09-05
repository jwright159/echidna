using System.Collections;
using BepuPhysics;
using Vector3System = System.Numerics.Vector3;
using Vector3OpenTK = OpenTK.Mathematics.Vector3;
// ReSharper disable CompareOfFloatsByEqualityOperator

namespace Echidna.Mathematics;

public struct Vector3 : IEquatable<Vector3>, IEnumerable<float>
{
	public float X;
	public float Y;
	public float Z;
	
	public float Length => MathF.Sqrt(LengthSquared);
	public float LengthSquared => X * X + Y * Y + Z * Z;
	public Vector3 Normalized => this / Length;
	
	public Vector3(float x, float y, float z)
	{
		X = x;
		Y = y;
		Z = z;
	}
	
	public override int GetHashCode() => HashCode.Combine(X, Y, Z);
	public override bool Equals(object? obj) => obj is Vector3 other && other == this;
	public bool Equals(Vector3 other) => other == this;
	public override string ToString() => $"<{X}, {Y}, {Z}>";
	
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	public IEnumerator<float> GetEnumerator()
	{
		yield return X;
		yield return Y;
		yield return Z;
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
	public static Vector3 operator-(Vector3 a, Vector3 b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
	public static Vector3 operator*(Vector3 vector, float scalar) => new(vector.X * scalar, vector.Y * scalar, vector.Z * scalar);
	public static Vector3 operator*(float scalar, Vector3 vector) => new(vector.X * scalar, vector.Y * scalar, vector.Z * scalar);
	public static Vector3 operator/(Vector3 vector, float scalar) => new(vector.X / scalar, vector.Y / scalar, vector.Z / scalar);
	public static bool operator==(Vector3 a, Vector3 b) => a.X == b.X && a.Y == b.Y && a.Z == b.Z;
	public static bool operator!=(Vector3 a, Vector3 b) => !(a == b);
	
	public static implicit operator Vector3System(Vector3 vector) => new(vector.X, vector.Y, vector.Z);
	public static implicit operator Vector3(Vector3System vector) => new(vector.X, vector.Y, vector.Z);
	public static implicit operator Vector3OpenTK(Vector3 vector) => new(vector.X, vector.Y, vector.Z);
	public static implicit operator Vector3(Vector3OpenTK vector) => new(vector.X, vector.Y, vector.Z);
	public static implicit operator Vector3((float X, float Y, float Z) vector) => new(vector.X, vector.Y, vector.Z);
	public static implicit operator RigidPose(Vector3 vector) => new(vector);
	
	public static float Distance(Vector3 a, Vector3 b) => (a - b).Length;
	public static float DistanceSquared(Vector3 a, Vector3 b) => (a - b).LengthSquared;
	public static Vector3 UnitFromTo(Vector3 a, Vector3 b) => (b - a).Normalized;
	
	public static Vector3 Sum<T>(IEnumerable<T> source, Func<T, Vector3> selector) => source.Aggregate(Zero, (current, item) => current + selector(item));
}