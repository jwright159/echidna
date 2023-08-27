using OpenTK.Mathematics;

namespace Echidna;

public class Camera
{
	private Matrix4 transform = Matrix4.LookAt((0, 0, -10), (0, 0, 0), Vector3.UnitY);
	
	private float fov = MathHelper.PiOver2;
	private float Fov
	{
		get => MathHelper.RadiansToDegrees(fov);
		set => fov = MathHelper.DegreesToRadians(MathHelper.Clamp(value, 1f, 180f));
	}
	
	public float AspectRatio { get; set; } = 16f / 9f;
	
	//public Matrix4 ViewMatrix => Matrix4.LookAt(Position, Position + _front, _up);
	public Matrix4 ViewMatrix => transform;
	public Matrix4 ProjectionMatrix => Matrix4.CreatePerspectiveFieldOfView(fov, AspectRatio, 0.01f, 100f);
}