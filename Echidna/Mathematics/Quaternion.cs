using System.Runtime.CompilerServices;
using QuaternionSystem = System.Numerics.Quaternion;
using QuaternionOpenTK = OpenTK.Mathematics.Quaternion;
// ReSharper disable CompareOfFloatsByEqualityOperator

namespace Echidna.Mathematics;

public struct Quaternion
{
	public float X;
	public float Y;
	public float Z;
	public float W;
	
	public Vector3 XYZ => new(X, Y, Z);
	
	public Quaternion(float x, float y, float z, float w)
	{
		X = x;
		Y = y;
		Z = z;
		W = w;
	}
	
	public override string ToString() => $"<{X}, {Y}, {Z}, {W}>";
	
	public Quaternion(Vector3 xyz, float w) : this(xyz.X, xyz.Y, xyz.Z, w) { }
	
	public static Quaternion Identity => new(0, 0, 0, 1);
	
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 operator *(Quaternion quaternion, Vector3 vector)
	{
		float x2 = quaternion.X + quaternion.X;
		float y2 = quaternion.Y + quaternion.Y;
		float z2 = quaternion.Z + quaternion.Z;
		
		float wx2 = quaternion.W * x2;
		float wy2 = quaternion.W * y2;
		float wz2 = quaternion.W * z2;
		float xx2 = quaternion.X * x2;
		float xy2 = quaternion.X * y2;
		float xz2 = quaternion.X * z2;
		float yy2 = quaternion.Y * y2;
		float yz2 = quaternion.Y * z2;
		float zz2 = quaternion.Z * z2;
		
		return new Vector3(
			vector.X * (1.0f - yy2 - zz2) + vector.Y * (xy2 - wz2) + vector.Z * (xz2 + wy2),
			vector.X * (xy2 + wz2) + vector.Y * (1.0f - xx2 - zz2) + vector.Z * (yz2 - wx2),
			vector.X * (xz2 - wy2) + vector.Y * (yz2 + wx2) + vector.Z * (1.0f - xx2 - yy2)
		);
	}
	
	public static Quaternion operator *(Quaternion a, Quaternion b) => new(b.W * a.XYZ + a.W * b.XYZ + a.XYZ.Cross(b.XYZ), a.W * b.W - a.XYZ.Dot(b.XYZ));
	
	public static implicit operator QuaternionSystem(Quaternion quaternion) => new(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);
	public static implicit operator Quaternion(QuaternionSystem quaternion) => new(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);
	public static implicit operator QuaternionOpenTK(Quaternion quaternion) => new(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);
	public static implicit operator Quaternion(QuaternionOpenTK quaternion) => new(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);
	public static implicit operator Quaternion((float X, float Y, float Z) eulers) => FromEulerAngles(eulers);
	public static implicit operator Quaternion((float X, float Y, float Z, float W) quaternion) => new(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);
	
	public static Quaternion FromAxisAngle(Vector3 axis, float angle)
	{
		float halfAngle = DegreesToRadians(angle) * 0.5f;
		float sin = MathF.Sin(halfAngle);
		float cos = MathF.Cos(halfAngle);
		return new Quaternion(axis.X * sin, axis.Y * sin, axis.Z * sin, cos);
	}
	
	/// <summary>
	/// Rotates by roll, then pitch, then yaw.
	/// </summary>
	public static Quaternion FromEulerAngles(float pitch, float roll, float yaw)
	{
		float halfRoll = DegreesToRadians(roll) * 0.5f;
		float sinRoll = MathF.Sin(halfRoll);
		float cosRoll = MathF.Cos(halfRoll);
		
		float halfPitch = DegreesToRadians(pitch) * 0.5f;
		float sinPitch = MathF.Sin(halfPitch);
		float cosPitch = MathF.Cos(halfPitch);
		
		float halfYaw = DegreesToRadians(yaw) * 0.5f;
		float sinYaw = MathF.Sin(halfYaw);
		float cosYaw = MathF.Cos(halfYaw);
		
		return new Quaternion(
			cosYaw * sinPitch * cosRoll + sinYaw * cosPitch * sinRoll,
			cosYaw * cosPitch * sinRoll + sinYaw * sinPitch * cosRoll,
			sinYaw * cosPitch * cosRoll - cosYaw * sinPitch * sinRoll,
			cosYaw * cosPitch * cosRoll + sinYaw * sinPitch * sinRoll);
	}
	
	/// <summary>
	/// Rotates by roll (y), then pitch (x), then yaw (z).
	/// </summary>
	public static Quaternion FromEulerAngles(Vector3 angles) => FromEulerAngles(angles.X, angles.Y, angles.Z);
	
	public static Quaternion LookToward(Vector3 forward, Vector3 up)
	{
		// https://www.euclideanspace.com/maths/geometry/rotations/conversions/matrixToQuaternion/
		// maybe https://gamedev.net/forums/topic/648857-how-to-implement-lookrotation/5113120/ ?
		
		forward = forward.Normalized;
		up = up.Normalized;
		Vector3 right = up.Cross(forward);
		up = forward.Cross(right);
		
		float m00 = right.X;
		float m01 = right.Y;
		float m02 = right.Z;
		float m10 = forward.X;
		float m11 = forward.Y;
		float m12 = forward.Z;
		float m20 = up.X;
		float m21 = up.Y;
		float m22 = up.Z;
		
		// TODO: does not work, det(m) != 0
		Console.WriteLine();
		Console.WriteLine($"{right} {forward} {up}");
		Console.WriteLine($"{m00 * (m11 * m22 - m12 * m21) - m01 * (m10 * m22 - m12 * m20) + m02 * (m10 * m21 - m11 * m20)}");
		
		float trace = m00 + m11 + m22;
		if (trace > 0)
		{
			float s = MathF.Sqrt(trace + 1) * 2;
			Console.WriteLine($"a {s}");
			return new Quaternion(
				(m21 - m12) / s,
				(m02 - m20) / s,
				(m10 - m01) / s,
				0.25f * s);
		}
		else if (m00 > m11 && m00 > m22)
		{
			float s = MathF.Sqrt(1 + m00 - m11 - m22) * 2;
			Console.WriteLine($"b {s}");
			return new Quaternion(
				0.25f * s,
				(m01 + m10) / s,
				(m02 + m20) / s,
				(m21 - m12) / s);
		}
		else if (m11 > m22)
		{
			float s = MathF.Sqrt(1 + m11 - m00 - m22) * 2;
			Console.WriteLine($"c {s}");
			return new Quaternion(
				(m01 + m10) / s,
				0.25f * s,
				(m12 + m21) / s,
				(m02 - m20) / s);
		}
		else
		{
			float s = MathF.Sqrt(1 + m22 - m00 - m11) * 2;
			Console.WriteLine($"d {s}");
			return new Quaternion(
				(m02 + m20) / s,
				(m12 + m21) / s,
				0.25f * s,
				(m10 - m01) / s);
		}
	}
	public static Quaternion LookAt(Vector3 position, Vector3 target, Vector3 up) => LookToward(target - position, up);
	
	public static float DegreesToRadians(float angle) => angle / 180f * MathF.PI;
	public static float RadiansToDegrees(float angle) => angle * 180f / MathF.PI;
}