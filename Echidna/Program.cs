using BepuPhysics;
using BepuPhysics.Collidables;
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
using Mesh = Echidna.Rendering.Mesh;
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
		
		#region Shader definitions
		Shader pulseShader = new("Shaders/shader.vert", "Shaders/pulse.frag");
		world.AddComponent(new Entity(), pulseShader);
		
		Shader globalCoordsShader = new("Shaders/shader.vert", "Shaders/global-coords.frag");
		world.AddComponent(new Entity(), globalCoordsShader);
		
		Shader textureShader = new("Shaders/shader.vert", "Shaders/texture.frag");
		world.AddComponent(new Entity(), textureShader);
		
		Shader skyboxShader = new("Shaders/skybox.vert", "Shaders/cubemap.frag");
		world.AddComponent(new Entity(), skyboxShader);
		#endregion
		
		#region Texture definitions
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
		#endregion
		
		#region Mesh definitions
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
		
		Mesh box = new(new[]
		{
			-0.5f, -0.5f, -0.5f,
			-0.5f, -0.5f, +0.5f,
			-0.5f, +0.5f, -0.5f,
			-0.5f, +0.5f, +0.5f,
			+0.5f, -0.5f, -0.5f,
			+0.5f, -0.5f, +0.5f,
			+0.5f, +0.5f, -0.5f,
			+0.5f, +0.5f, +0.5f,
		}, new[]
		{
			1.0f, 0.0f,
			1.0f, 1.0f,
			0.0f, 0.0f,
			0.0f, 1.0f,
			1.0f, 0.0f,
			1.0f, 1.0f,
			0.0f, 0.0f,
			0.0f, 1.0f,
		}, new[]
		{
			0.0f, 0.0f, 0.0f,
			0.0f, 0.0f, 1.0f,
			0.0f, 1.0f, 0.0f,
			0.0f, 1.0f, 1.0f,
			1.0f, 0.0f, 0.0f,
			1.0f, 0.0f, 1.0f,
			1.0f, 1.0f, 0.0f,
			1.0f, 1.0f, 1.0f,
		}, new uint[]
		{
			0, 2, 1,
			1, 2, 3,
			2, 6, 3,
			3, 6, 7,
			4, 0, 5,
			5, 0, 1,
			5, 1, 7,
			7, 1, 3,
			6, 2, 4,
			4, 2, 0,
			6, 4, 7,
			7, 4, 5,
		});
		world.AddComponent(new Entity(), box);
		
		Mesh splitFacesBox = new(new[]
		{
			+0.5f, -0.5f, +0.5f,
			+0.5f, +0.5f, +0.5f,
			+0.5f, +0.5f, -0.5f,
			+0.5f, -0.5f, -0.5f,
			
			-0.5f, +0.5f, +0.5f,
			-0.5f, -0.5f, +0.5f,
			-0.5f, -0.5f, -0.5f,
			-0.5f, +0.5f, -0.5f,
			
			+0.5f, +0.5f, +0.5f,
			-0.5f, +0.5f, +0.5f,
			-0.5f, +0.5f, -0.5f,
			+0.5f, +0.5f, -0.5f,
			
			-0.5f, -0.5f, +0.5f,
			+0.5f, -0.5f, +0.5f,
			+0.5f, -0.5f, -0.5f,
			-0.5f, -0.5f, -0.5f,
			
			-0.5f, +0.5f, +0.5f,
			+0.5f, +0.5f, +0.5f,
			+0.5f, -0.5f, +0.5f,
			-0.5f, -0.5f, +0.5f,
			
			+0.5f, +0.5f, -0.5f,
			-0.5f, +0.5f, -0.5f,
			-0.5f, -0.5f, -0.5f,
			+0.5f, -0.5f, -0.5f,
		}, new[]
		{
			0.0f, 1.0f,
			1.0f, 1.0f,
			1.0f, 0.0f,
			0.0f, 0.0f,
			
			0.0f, 1.0f,
			1.0f, 1.0f,
			1.0f, 0.0f,
			0.0f, 0.0f,
			
			0.0f, 1.0f,
			1.0f, 1.0f,
			1.0f, 0.0f,
			0.0f, 0.0f,
			
			0.0f, 1.0f,
			1.0f, 1.0f,
			1.0f, 0.0f,
			0.0f, 0.0f,
			
			0.0f, 1.0f,
			1.0f, 1.0f,
			1.0f, 0.0f,
			0.0f, 0.0f,
			
			0.0f, 1.0f,
			1.0f, 1.0f,
			1.0f, 0.0f,
			0.0f, 0.0f,
		}, new[]
		{
			1.0f, 0.0f, 1.0f,
			1.0f, 1.0f, 1.0f,
			1.0f, 1.0f, 0.0f,
			1.0f, 0.0f, 0.0f,
			
			0.0f, 1.0f, 1.0f,
			0.0f, 0.0f, 1.0f,
			0.0f, 0.0f, 0.0f,
			0.0f, 1.0f, 0.0f,
			
			1.0f, 1.0f, 1.0f,
			0.0f, 1.0f, 1.0f,
			0.0f, 1.0f, 0.0f,
			1.0f, 1.0f, 0.0f,
			
			0.0f, 0.0f, 1.0f,
			1.0f, 0.0f, 1.0f,
			1.0f, 0.0f, 0.0f,
			0.0f, 0.0f, 0.0f,
			
			0.0f, 1.0f, 1.0f,
			1.0f, 1.0f, 1.0f,
			1.0f, 0.0f, 1.0f,
			0.0f, 0.0f, 1.0f,
			
			1.0f, 1.0f, 0.0f,
			0.0f, 1.0f, 0.0f,
			0.0f, 0.0f, 0.0f,
			1.0f, 0.0f, 0.0f,
		}, new uint[]
		{
			0, 1, 3,
			3, 1, 2,
			
			4, 5, 7,
			7, 5, 6,
			
			8, 9, 11,
			11, 9, 10,
			
			12, 13, 15,
			15, 13, 14,
			
			16, 17, 19,
			19, 17, 18,
			
			20, 21, 23,
			23, 21, 22,
		});
		world.AddComponent(new Entity(), splitFacesBox);
		
		IObjLoader objLoader = new ObjLoaderFactory().Create();
		Mesh sphere = LoadObj("Shaders/sphere.obj");
		world.AddComponent(new Entity(), sphere);
		#endregion
		
		Window window = new(gameWindow);
		world.AddSingletonComponent(window);
		
		Entity cameraEntity = new();
		Projection projection = new();
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
		
		//AddBody((0, 0, 5), (0, 30, 0));
		//AddBody((0, 0, -4), (0, 0, 0));
		
		world.AddComponent(new Entity(), new SkyboxRenderer(splitFacesBox, skyboxShader, skyboxCubeMap));
		
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
		
		void AddBody(Vector3 position, Quaternion rotation)
		{
			Entity entity = new();
			Transform transform = new() { LocalPosition = position, LocalRotation = rotation };
			Box shape = new(1, 1, 1);
			BodyInertia inertia = shape.ComputeInertia(1);
			GravitationalField gravity = new(20f, transform, gravitationalFields);
			world.AddComponent(entity, new SimulationTarget(simulation));
			world.AddComponent<BodyShape>(entity, new BodyShape<Box>(shape));
			world.AddComponent(entity, new DynamicBody(inertia));
			world.AddComponent(entity, transform);
			world.AddComponent(entity, new MeshRenderer(splitFacesBox, textureShader, crateTexture));
			world.AddComponent(entity, gravity);
			world.AddComponent(entity, new AffectedByGravity(gravitationalFields, gravity));
		}
		
		Mesh LoadObj(string filename)
		{
			using Stream fileStream = File.OpenRead(filename);
			LoadResult result = objLoader.Load(fileStream);
			return new Mesh(
				result.Vertices.SelectMany(vertex => EnumerableOf(vertex.X, vertex.Y, vertex.Z)).ToArray(),
				result.Textures.SelectMany(vertex => EnumerableOf(vertex.X, vertex.Y)).ToArray(),
				result.Vertices.SelectMany(_ => EnumerableOf(1f, 1f, 1f)).ToArray(),
				result.Groups.SelectMany(group => group.Faces.SelectMany(face => EnumerableOf((uint)face[0].VertexIndex - 1, (uint)face[1].VertexIndex - 1, (uint)face[2].VertexIndex - 1))).ToArray());
		}
	}
	
	private static IEnumerable<T> EnumerableOf<T>(params T[] items) => items;
}
