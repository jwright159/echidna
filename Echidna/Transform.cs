using OpenTK.Mathematics;

namespace Echidna;

public class Transform : Component
{
	public Vector3 LocalPosition { get; set; }
	
	public Matrix4 Transformation => Matrix4.CreateTranslation(LocalPosition);
}