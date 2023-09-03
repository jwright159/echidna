using System.Numerics;
using System.Runtime.CompilerServices;
using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPhysics.CollisionDetection;
using BepuPhysics.Constraints;
using BepuUtilities;
using BepuUtilities.Memory;

namespace Echidna.Physics;

public class WorldSimulationSystem : System<WorldSimulation>
{
	protected override void OnInitializeEach(WorldSimulation simulation)
	{
		simulation.BufferPool = new BufferPool();
		simulation.Simulation = Simulation.Create(simulation.BufferPool, new NarrowPhaseCallbacks(), new PoseIntegratorCallbacks(new Vector3(0, 0, -10)), new SolveDescription(8, 1));
		simulation.ThreadDispatcher = new ThreadDispatcher(Environment.ProcessorCount);
	}
	
	protected override void OnPhysicsUpdateEach(float deltaTime, WorldSimulation simulation)
	{
		simulation.Simulation!.Timestep(deltaTime, simulation.ThreadDispatcher);
	}
	
	protected override void OnDisposeEach(WorldSimulation simulation)
	{
		simulation.HasBeenDisposed = true;
		simulation.Simulation!.Dispose();
		simulation.ThreadDispatcher!.Dispose();
		simulation.BufferPool!.Clear();
	}
	
	private struct NarrowPhaseCallbacks : INarrowPhaseCallbacks
	{
		public void Initialize(Simulation simulation)
		{
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool AllowContactGeneration(int workerIndex, CollidableReference a, CollidableReference b, ref float speculativeMargin)
		{
			return a.Mobility == CollidableMobility.Dynamic || b.Mobility == CollidableMobility.Dynamic;
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool AllowContactGeneration(int workerIndex, CollidablePair pair, int childIndexA, int childIndexB)
		{
			return true;
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool ConfigureContactManifold<TManifold>(int workerIndex, CollidablePair pair, ref TManifold manifold, out PairMaterialProperties pairMaterial) where TManifold : unmanaged, IContactManifold<TManifold>
		{
			pairMaterial.FrictionCoefficient = 1f;
			pairMaterial.MaximumRecoveryVelocity = 2f;
			pairMaterial.SpringSettings = new SpringSettings(30, 1);
			return true;
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool ConfigureContactManifold(int workerIndex, CollidablePair pair, int childIndexA, int childIndexB, ref ConvexContactManifold manifold)
		{
			return true;
		}
		
		public void Dispose()
		{
		}
	}
	
	private struct PoseIntegratorCallbacks : IPoseIntegratorCallbacks
	{
		private Vector3 gravity;
		private Vector3Wide gravityWideDt;
		
		public readonly AngularIntegrationMode AngularIntegrationMode => AngularIntegrationMode.Nonconserving;
		
		public readonly bool AllowSubstepsForUnconstrainedBodies => false;
		
		public readonly bool IntegrateVelocityForKinematics => false;
		
		public PoseIntegratorCallbacks(Vector3 gravity) : this()
		{
			this.gravity = gravity;
		}
		
		public void Initialize(Simulation simulation)
		{
		}
		
		public void PrepareForIntegration(float dt)
		{
			gravityWideDt = Vector3Wide.Broadcast(gravity * dt);
		}
		
		public void IntegrateVelocity(Vector<int> bodyIndices, Vector3Wide position, QuaternionWide orientation, BodyInertiaWide localInertia, Vector<int> integrationMask, int workerIndex, Vector<float> dt, ref BodyVelocityWide velocity)
		{
			velocity.Linear += gravityWideDt;
		}
	}
}