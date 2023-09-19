using BepuPhysics;
using Echidna.Core;

namespace Echidna.Physics;

public class SimulationTarget : EntityComponent
{
	internal readonly WorldSimulation WorldSimulation;
	
	public Simulation Simulation => WorldSimulation.Simulation!;
	
	public SimulationTarget(WorldSimulation worldSimulation)
	{
		WorldSimulation = worldSimulation;
	}
}