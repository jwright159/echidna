using OpenTK.Mathematics;
using Vector3 = Echidna.Mathematics.Vector3;

namespace Echidna.Rendering;

public class FirstPersonCamera : Component
{
	private float pitch;
	public float Pitch
	{
		get => pitch;
		set => pitch = MathHelper.Clamp(value, -90f, 90f);
	}
	
	public float Yaw { get; set; }
	
	public float movementSpeed = 1f;
	public float mouseSensitivity = 1f;
	
	public Vector3 movement = Vector3.Zero;
	
	public Quaternion Rotation => Quaternion.FromAxisAngle(Vector3.Up, MathHelper.DegreesToRadians(-Yaw)) * Quaternion.FromAxisAngle(Vector3.Right, MathHelper.DegreesToRadians(-Pitch));
}