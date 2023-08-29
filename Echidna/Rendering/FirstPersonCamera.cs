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
	
	public Quaternion Rotation => Quaternion.FromEulerAngles(Pitch, Yaw, 0);
}