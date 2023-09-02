using BepuPhysics;

namespace Echidna.Physics;

public class SimulationBody : Component
{
	internal readonly WorldSimulation simulation;
	internal BodyHandle handle;
	
	public SimulationBody(WorldSimulation simulation)
	{
		this.simulation = simulation;
	}
}