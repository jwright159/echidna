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
	
	public Quaternion(float x, float y, float z, float w)
	{
		X = x;
		Y = y;
		Z = z;
		W = w;
	}
	
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
	
	public static float DegreesToRadians(float angle) => angle / 180f * MathF.PI;
	public static float RadiansToDegrees(float angle) => angle * 180f / MathF.PI;
}