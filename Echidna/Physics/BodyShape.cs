using BepuPhysics;
using BepuPhysics.Collidables;

namespace Echidna.Physics;

public abstract class BodyShape : Component
{
	internal readonly WorldSimulation Simulation;
	
	protected BodyShape(WorldSimulation simulation)
	{
		Simulation = simulation;
	}
	
	public abstract TypedIndex AddToSimulation();
}

public class BodyShape<TShape> : BodyShape where TShape : unmanaged, IShape
{
	internal readonly TShape Shape;
	
	public BodyShape(WorldSimulation simulation, TShape shape) : base(simulation)
	{
		Shape = shape;
	}
	
	public override TypedIndex AddToSimulation() => Simulation.Simulation!.Shapes.Add(Shape);
}