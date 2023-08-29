using OpenTK.Mathematics;

namespace Echidna.Rendering;

public class Projection : Component
{
	private float fov = 90f;
	public float Fov
	{
		get => fov;
		set => fov = MathHelper.Clamp(value, 1f, 180f);
	}
	
	public float AspectRatio { get; set; } = 16f / 9f;
	
	public Matrix4 ProjectionMatrix => Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(fov), AspectRatio, 0.01f, 100f);
}