using Echidna.Mathematics;

namespace Echidna.Rendering;

public class FirstPersonCamera : Component
{
	private float pitch;
	public float Pitch
	{
		get => pitch;
		set => pitch = Math.Clamp(value, -90f, 90f);
	}
	
	public float Yaw { get; set; }
	
	public float MovementSpeed = 1f;
	public float MouseSensitivity = 1f;
	
	public Vector3 Movement = Vector3.Zero;
	
	public Quaternion Rotation => Quaternion.FromEulerAngles(-Pitch, 0, -Yaw);
}