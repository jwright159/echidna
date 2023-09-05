using BepuPhysics;
using BepuPhysics.Collidables;
using BepuUtilities.Memory;
using Echidna.Core;
using Echidna.Hierarchy;
using Echidna.Input;
using Echidna.Mathematics;
using Echidna.Physics;
using Echidna.Rendering;
using ObjLoader.Loader.Loaders;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using MeshShape = BepuPhysics.Collidables.Mesh;
using Mesh = Echidna.Rendering.Mesh;
using Texture = Echidna.Rendering.Texture;
using Window = Echidna.Rendering.Window;
using Vector2i = OpenTK.Mathematics.Vector2i;

namespace Echidna.Demo;

public static class Program
{
	private static void Main()
	{
		Vector2i size = (1080, 720);
		using GameWindow gameWindow = new(
			new GameWindowSettings(),
			new NativeWindowSettings
			{
				Size = size,
			});
		gameWindow.CursorState = CursorState.Grabbed;
		
		World world = new(
			new ResizeWindowSystem(),
			new ClearScreenSystem(),
			new ShaderSystem(),
			new TextureSystem(),
			new CubeMapSystem(),
			
			new LifetimeSystem(),
			new WorldSimulationSystem(),
			new DynamicBodyTransformSystem(),
			new StaticBodyTransformSystem(),
			
			new GravitySystem(),
			
			new InputSystem(),
			new SpinnerSystem(),
			new FirstPersonCameraSystem(),
			new PulsatingShaderSystem(),
			
			new CameraShaderProjectionSystem(),
			new MeshSystem(),
			new MeshRendererSystem(),
			new SkyboxRendererSystem(),
			
			new SwapBuffersSystem());
		
		Shader pulseShader = new("Shaders/shader.vert", "Shaders/pulse.frag");
		world.AddComponent(new Entity(), pulseShader);
		
		Shader globalCoordsShader = new("Shaders/shader.vert", "Shaders/global-coords.frag");
		world.AddComponent(new Entity(), globalCoordsShader);
		
		Shader textureShader = new("Shaders/shader.vert", "Shaders/texture.frag");
		world.AddComponent(new Entity(), textureShader);
		
		Shader skyboxShader = new("Shaders/skybox.vert", "Shaders/cubemap.frag");
		world.AddComponent(new Entity(), skyboxShader);
		
		Texture crateTexture = new("Shaders/container.jpg");
		world.AddComponent(new Entity(), crateTexture);
		
		CubeMap skyboxCubeMap = new(
			"Shaders/Skybox/right.png",
			"Shaders/Skybox/left.png",
			"Shaders/Skybox/front.png",
			"Shaders/Skybox/back.png",
			"Shaders/Skybox/top.png",
			"Shaders/Skybox/bottom.png");
		world.AddComponent(new Entity(), skyboxCubeMap);
		
		Mesh triangle = new(new[]
		{
			+0.5f, +0.0f, -0.5f,
			-0.5f, +0.0f, -0.5f,
			+0.0f, +0.0f, +0.5f,
		}, new[]
		{
			1.0f, 0.0f,
			0.0f, 0.0f,
			0.5f, 1.0f,
		}, new[]
		{
			1.0f, 0.0f, 0.0f,
			0.0f, 1.0f, 0.0f,
			0.0f, 0.0f, 1.0f,
		}, new uint[]
		{
			0, 1, 2,
		}, false);
		world.AddComponent(new Entity(), triangle);
		
		Mesh cube = LoadObj("Shaders/cube.obj");
		world.AddComponent(new Entity(), cube);
		
		Mesh sphere = LoadObj("Shaders/sphere.obj");
		world.AddComponent(new Entity(), sphere);
		
		Window window = new(gameWindow);
		world.AddSingletonComponent(window);
		
		Entity cameraEntity = new();
		Projection projection = new(){ DepthFar = 10000f };
		world.AddComponent(cameraEntity, new Transform{ LocalPosition = (0, -5, 0) });
		world.AddComponent(cameraEntity, projection);
		
		world.AddComponent(cameraEntity, new Lifetime());
		world.AddComponent(cameraEntity, new CameraShaders(pulseShader, globalCoordsShader, textureShader, skyboxShader));
		world.AddComponent(cameraEntity, new PulsatingShader(pulseShader));
		world.AddComponent(cameraEntity, new CameraResizer(window, projection, size));
		
		FirstPersonCamera firstPerson = new(){ MouseSensitivity = 0.5f, MovementSpeed = 1.5f };
		world.AddComponent(cameraEntity, firstPerson);
		world.AddComponent(cameraEntity, new InputGroup(
			new InputAction<Vector3>(value => firstPerson.Movement = value,
				new Axis3InputTrigger(
					new AxisInputTrigger(
						new SingleInputTrigger(Keys.D),
						new SingleInputTrigger(Keys.A)),
					new AxisInputTrigger(
						new SingleInputTrigger(Keys.W),
						new SingleInputTrigger(Keys.S)),
					new AxisInputTrigger(
						new SingleInputTrigger(Keys.E),
						new SingleInputTrigger(Keys.Q)))),
			new InputAction<float>(value => firstPerson.Pitch += value * firstPerson.MouseSensitivity,
				new SingleInputTrigger(MouseAxis.DeltaY)),
			new InputAction<float>(value => firstPerson.Yaw += value * firstPerson.MouseSensitivity,
				new SingleInputTrigger(MouseAxis.DeltaX)),
			new InputAction<float>(value => Console.WriteLine($"Space {value}"),
				new SingleInputTrigger(Keys.Space)),
			// ReSharper disable once AccessToDisposedClosure
			new InputAction<float>(_ => gameWindow.Close(),
				new SingleInputTrigger(Keys.Escape))));
		
		AddMesh((2, 0, 0), (MathF.PI / 4f, 0, 0), triangle, globalCoordsShader, null);
		AddMesh((0, 2, 0), (0, MathF.PI / 4f, 0), triangle, globalCoordsShader, null);
		AddMesh((0, 0, 2), (0, 0, MathF.PI / 4f), triangle, globalCoordsShader, null);
		AddMesh((0, 0, 0), (0, 0, 0), sphere, globalCoordsShader, null);
		
		WorldSimulation simulation = new();
		world.AddSingletonComponent(simulation);
		
		GravitationalFields gravitationalFields = new();
		world.AddSingletonComponent(gravitationalFields);
		
		AddPlanet((0, 0, -10_005), 10_000);
		AddBox((-2, 0, 5), (0, 30, 0));
		AddBox((-4, 0, 3), (0, 0, 0));
		
		world.AddComponent(new Entity(), new SkyboxRenderer(cube, skyboxShader, skyboxCubeMap));
		
		gameWindow.Load += world.Initialize;
		gameWindow.Unload += world.Dispose;
		gameWindow.UpdateFrame += args => world.Update((float)args.Time);
		gameWindow.RenderFrame += _ => world.Draw();
		gameWindow.MouseMove += args => world.MouseMove(args.Position, args.Delta);
		gameWindow.KeyDown += args => world.KeyDown(args.Key);
		gameWindow.KeyUp += args => world.KeyUp(args.Key);
		gameWindow.Run();
		return;
		
		void AddMesh(Vector3 position, Vector3 rotation, Mesh mesh, Shader shader, Texture? texture)
		{
			Entity entity = new();
			world.AddComponent(entity, new MeshRenderer(mesh, shader, texture));
			world.AddComponent(entity, new Transform{ LocalPosition = position, LocalRotation = Quaternion.FromEulerAngles(rotation) });
			world.AddComponent(entity, new Spinner(rotation, Quaternion.RadiansToDegrees(rotation.Length)));
		}
		
		void AddBox(Vector3 position, Quaternion rotation)
		{
			Entity entity = new();
			Transform transform = new() { LocalPosition = position, LocalRotation = rotation };
			Box shape = new(2, 2, 2);
			BodyInertia inertia = shape.ComputeInertia(1);
			world.AddComponent(entity, new SimulationTarget(simulation));
			world.AddComponent(entity, BodyShape.Of(shape));
			world.AddComponent(entity, new DynamicBody(inertia));
			world.AddComponent(entity, transform);
			world.AddComponent(entity, new MeshRenderer(cube, textureShader, crateTexture));
			world.AddComponent(entity, new AffectedByGravity(gravitationalFields));
		}
		
		void AddPlanet(Vector3 position, float radius)
		{
			Entity entity = new();
			Transform transform = new() { LocalPosition = position, LocalScale = Vector3.One * radius };
			ConvexHull shape = new(new Span<System.Numerics.Vector3>(sphere.Positions.Chunk(3).Select(vertex => new System.Numerics.Vector3(vertex[0], vertex[1], vertex[2]) * radius).ToArray()), new BufferPool(), out _);
			GravitationalField gravity = new(1000000000f, transform, gravitationalFields);
			world.AddComponent(entity, new SimulationTarget(simulation));
			world.AddComponent(entity, BodyShape.Of(shape));
			world.AddComponent(entity, new StaticBody());
			world.AddComponent(entity, transform);
			world.AddComponent(entity, new MeshRenderer(sphere, textureShader, crateTexture));
			world.AddComponent(entity, gravity);
			world.AddComponent(entity, new AffectedByGravity(gravitationalFields));
		}
		
		Mesh LoadObj(string filename)
		{
			using Stream fileStream = File.OpenRead(filename);
			LoadResult result = new ObjLoaderFactory().Create().Load(fileStream);
			
			List<ObjVertex> uniqueVertices = new(); // this is probably not going to hold up, figure out a set with insertion order
			uint[] faces = result.Groups
				.SelectMany(group => group.Faces
					.SelectMany(face => Enumerable.Range(0, face.Count)
						.Select(i =>
						{
							ObjLoader.Loader.Data.VertexData.Vertex vertex = result.Vertices[face[i].VertexIndex - 1];
							ObjLoader.Loader.Data.VertexData.Texture texCoord = result.Textures[face[i].TextureIndex - 1];
							uniqueVertices.Add(new ObjVertex
							{
								X = vertex.X,
								Y = vertex.Y,
								Z = vertex.Z,
								U = texCoord.X,
								V = texCoord.Y,
							});
							return (uint)uniqueVertices.Count - 1;
						})
					)
				).ToArray();
			
			return new Mesh(
				uniqueVertices.SelectMany(vertex => EnumerableOf(vertex.X, vertex.Y, vertex.Z)).ToArray(),
				uniqueVertices.SelectMany(vertex => EnumerableOf(vertex.U, vertex.V)).ToArray(),
				uniqueVertices.SelectMany(_ => EnumerableOf(1f, 1f, 1f)).ToArray(),
				faces);
		}
	}
	
	private static IEnumerable<T> EnumerableOf<T>(params T[] items) => items;
}

public struct ObjVertex
{
	public float X;
	public float Y;
	public float Z;
	public float U;
	public float V;
}