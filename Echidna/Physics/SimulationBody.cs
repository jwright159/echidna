using BepuPhysics;

namespace Echidna.Physics;

public class SimulationBody : Component
{
	internal readonly WorldSimulation Simulation;
	internal BodyHandle Handle;
	
	public SimulationBody(WorldSimulation simulation)
	{
		Simulation = simulation;
	}
}