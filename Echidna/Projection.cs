using OpenTK.Mathematics;

namespace Echidna;

public class Projection : Component
{
	private float fov = MathHelper.PiOver2;
	private float Fov
	{
		get => MathHelper.RadiansToDegrees(fov);
		set => fov = MathHelper.DegreesToRadians(MathHelper.Clamp(value, 1f, 180f));
	}
	
	public float AspectRatio { get; set; } = 16f / 9f;
	
	public Matrix4 ProjectionMatrix => Matrix4.CreatePerspectiveFieldOfView(fov, AspectRatio, 0.01f, 100f);
}