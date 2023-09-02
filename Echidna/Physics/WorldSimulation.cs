using BepuPhysics;
using BepuUtilities;
using BepuUtilities.Memory;

namespace Echidna.Physics;

public class WorldSimulation : Component
{
	internal BufferPool? bufferPool;
	internal ThreadDispatcher? threadDispatcher;
	internal Simulation? simulation;
	
	internal bool hasBeenDisposed;
	
	~WorldSimulation()
	{
		if (!hasBeenDisposed)
			Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
	}
}