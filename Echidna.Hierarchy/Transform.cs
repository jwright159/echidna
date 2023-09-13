using Echidna.Core;
using Echidna.Mathematics;
using Matrix4 = OpenTK.Mathematics.Matrix4;

namespace Echidna.Hierarchy;

public class Transform : Component
{
	public Vector3 LocalPosition { get; set; } = Vector3.Zero;
	public Quaternion LocalRotation { get; set; } = Quaternion.Identity;
	public Vector3 LocalScale { get; set; } = Vector3.One;
	
	public Matrix4 Transformation => Matrix4.CreateScale(LocalScale) * Matrix4.CreateFromQuaternion(LocalRotation) * Matrix4.CreateTranslation(LocalPosition);
	
	public Vector3 TransformDirection(Vector3 direction) => LocalRotation * direction;
}