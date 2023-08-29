using OpenTK.Mathematics;

namespace Echidna.Hierarchy;

public class Transform : Component
{
	public Vector3 LocalPosition { get; set; } = Vector3.Zero;
	public Quaternion LocalRotation { get; set; } = Quaternion.Identity;
	
	public Matrix4 Transformation => Matrix4.CreateFromQuaternion(LocalRotation) * Matrix4.CreateTranslation(LocalPosition);
}