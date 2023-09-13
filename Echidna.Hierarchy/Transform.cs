using Echidna.Core;
using Echidna.Mathematics;
using Matrix4 = OpenTK.Mathematics.Matrix4;

namespace Echidna.Hierarchy;

public class Transform : Component
{
	private Vector3 localPosition = Vector3.Zero;
	public Vector3 LocalPosition
	{
		get => localPosition;
		set
		{
			localPosition = value;
			RecalculateLocalTransformation();
		}
	}
	
	private Vector3 position = Vector3.Zero;
	public Vector3 Position
	{
		get => position;
	}
	
	private Quaternion localRotation = Quaternion.Identity;
	public Quaternion LocalRotation
	{
		get => localRotation;
		set
		{
			localRotation = value;
			RecalculateLocalTransformation();
		}
	}
	
	private Quaternion rotation = Quaternion.Identity;
	public Quaternion Rotation
	{
		get => rotation;
	}
	
	private Vector3 localScale = Vector3.One;
	public Vector3 LocalScale
	{
		get => localScale;
		set
		{
			localScale = value;
			RecalculateLocalTransformation();
		}
	}
	
	private Transform? parent;
	private List<Transform> children = new();
	public Transform? Parent
	{
		get => parent;
		set
		{
			CheckForRecursion(value);
			parent?.children.Remove(this);
			parent = value;
			parent?.children.Add(this);
			RecalculateTransformation();
		}
	}
	private void CheckForRecursion(Transform? root)
	{
		if (root == null) return;
		if (root == this) throw new ArgumentException($"{root} cannot be a parent of itself");
		while (root.parent != null)
		{
			root = root.parent;
			if (root == this) throw new ArgumentException($"{root} cannot be a parent of its ancestor {this}");
		}
	}
	
	public Matrix4 LocalTransformation { get; private set; } = Matrix4.Identity;
	private void RecalculateLocalTransformation()
	{
		LocalTransformation = Matrix4.CreateScale(localScale) * Matrix4.CreateFromQuaternion(localRotation) * Matrix4.CreateTranslation(localPosition);
		RecalculateTransformation();
	}
	
	public Matrix4 Transformation { get; private set; } = Matrix4.Identity;
	private void RecalculateTransformation()
	{
		Transformation = parent != null ? LocalTransformation * parent.Transformation : LocalTransformation;
		position = Transformation.ExtractTranslation();
		rotation = Transformation.ExtractRotation();
		
		foreach (Transform child in children)
			child.RecalculateTransformation();
	}
	
	public Vector3 TransformDirection(Vector3 direction) => new(
		Transformation.M11 * direction.X + Transformation.M21 * direction.Y + Transformation.M31 * direction.Z,
		Transformation.M12 * direction.X + Transformation.M22 * direction.Y + Transformation.M32 * direction.Z,
		Transformation.M13 * direction.X + Transformation.M23 * direction.Y + Transformation.M33 * direction.Z
	);
	public Vector3 TransformPoint(Vector3 point) => new(
		Transformation.M11 * point.X + Transformation.M21 * point.Y + Transformation.M31 * point.Z + Transformation.M41,
		Transformation.M12 * point.X + Transformation.M22 * point.Y + Transformation.M32 * point.Z + Transformation.M42,
		Transformation.M13 * point.X + Transformation.M23 * point.Y + Transformation.M33 * point.Z + Transformation.M43
	);
}