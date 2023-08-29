using OpenTK.Mathematics;

namespace Echidna.Rendering;

public class FirstPersonCamera : Component
{
	private float pitch;
	public float Pitch
	{
		get => pitch;
		set => pitch = MathHelper.Clamp(value, 0f, 180f);
	}
	
	public float Yaw { get; set; }

	public float movementSpeed = 1f;
	public float mouseSensitivity = 1f;
	
	public Vector3 movement = Vector3.Zero;
	
	public Quaternion Rotation => Quaternion.FromEulerAngles(Pitch, Yaw, 0);
}