using BepuPhysics;
using BepuUtilities;
using BepuUtilities.Memory;
using Echidna.Core;

namespace Echidna.Physics;

public class WorldSimulation : Component
{
	internal BufferPool? BufferPool;
	internal ThreadDispatcher? ThreadDispatcher;
	internal Simulation? Simulation;
	
	internal bool HasBeenDisposed;
	
	~WorldSimulation()
	{
		if (!HasBeenDisposed)
			Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
	}
}