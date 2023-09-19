using Echidna.Core;
using OpenTK.Mathematics;

namespace Echidna.Rendering.Shader;

public class Perspective : EntityComponent
{
	private float fov = 90f;
	public float Fov
	{
		get => fov;
		set => fov = MathHelper.Clamp(value, 1f, 180f);
	}
	
	public float AspectRatio { get; set; } = 16f / 9f;
	
	public float DepthNear { get; set; } = 0.01f;
	public float DepthFar { get; set; } = 100f;
	
	public Matrix4 ProjectionMatrix => Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(fov), AspectRatio, DepthNear, DepthFar);
}