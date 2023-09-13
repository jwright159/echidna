using BepuPhysics.Collidables;
using Echidna.Core;

namespace Echidna.Physics;

public abstract class BodyShape : Component
{
	public abstract TypedIndex AddToShapes(Shapes shapes);
	
	public static BodyShape Of<TShape>(TShape shape)  where TShape : unmanaged, IShape => new BodyShape<TShape>(shape);
}

public class BodyShape<TShape> : BodyShape where TShape : unmanaged, IShape
{
	internal readonly TShape Shape;
	
	public BodyShape(TShape shape)
	{
		Shape = shape;
	}
	
	public override TypedIndex AddToShapes(Shapes shapes) => shapes.Add(Shape);
}