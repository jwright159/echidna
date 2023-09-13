using System.Drawing;
using BepuPhysics;
using BepuPhysics.Collidables;
using BepuUtilities.Memory;
using Echidna.Core;
using Echidna.Hierarchy;
using Echidna.Input;
using Echidna.Mathematics;
using Echidna.Physics;
using Echidna.Rendering.Font;
using Echidna.Rendering.Mesh;
using Echidna.Rendering.Shader;
using Echidna.Rendering.Texture;
using Echidna.Rendering.Window;
using ObjLoader.Loader.Loaders;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using MeshShape = BepuPhysics.Collidables.Mesh;
using Mesh = Echidna.Rendering.Mesh.Mesh;
using Texture = Echidna.Rendering.Texture.Texture;
using Window = Echidna.Rendering.Window.Window;
using Vector2i = OpenTK.Mathematics.Vector2i;

namespace Echidna.Editor;

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
			new FontSystem(),
			
			new LifetimeSystem(),
			new WorldSimulationSystem(),
			new DynamicBodyTransformSystem(),
			new StaticBodyTransformSystem(),
			
			new GravitySystem(),
			
			new InputSystem(),
			new SpinnerSystem(),
			new FirstPersonCameraSystem(),
			new LookAtSystem(),
			new PulsatingShaderSystem(),
			
			new CameraShader2dSystem(),
			new CameraShaderPerspectiveSystem(),
			new MeshSystem(),
			new MeshRendererSystem(),
			new SkyboxRendererSystem(),
			new FontRendererSystem(),
			
			new SwapBuffersSystem());
		
		Shader pulseShader = new(ShaderNodeUtil.MainVertexShader, File.ReadAllText("Assets/pulse.frag"));
		world.AddComponent(new Entity(), pulseShader);
		
		Shader globalCoordsShader = new(ShaderNodeUtil.MainVertexShader, File.ReadAllText("Assets/global-coords.frag"));
		world.AddComponent(new Entity(), globalCoordsShader);
		
		Shader globalCoords2dShader = new(ShaderNodeUtil.MainVertexShader, File.ReadAllText("Assets/global-coords.frag"));
		world.AddComponent(new Entity(), globalCoords2dShader);
		
		Shader textureShader = new(ShaderNodeUtil.MainVertexShader, File.ReadAllText("Assets/texture.frag"));
		world.AddComponent(new Entity(), textureShader);
		
		Shader font2dShader = new(ShaderNodeUtil.MainVertexShader, File.ReadAllText("Assets/font.frag"));
		world.AddComponent(new Entity(), font2dShader);
		
		Shader font3dShader = new(ShaderNodeUtil.MainVertexShader, File.ReadAllText("Assets/font.frag"));
		world.AddComponent(new Entity(), font3dShader);
		
		Shader skyboxShader = new(ShaderNodeUtil.SkyboxVertexShader, ShaderNodeUtil.CubeMapFragmentShader);
		world.AddComponent(new Entity(), skyboxShader);
		
		Texture crateTexture = new("Assets/container.jpg");
		world.AddComponent(new Entity(), crateTexture);
		
		CubeMap skyboxCubeMap = new(
			"Assets/Skybox/right.png",
			"Assets/Skybox/left.png",
			"Assets/Skybox/front.png",
			"Assets/Skybox/back.png",
			"Assets/Skybox/top.png",
			"Assets/Skybox/bottom.png");
		world.AddComponent(new Entity(), skyboxCubeMap);
		
		Font cascadiaCode = new("Assets/CascadiaCode.ttf");
		world.AddComponent(new Entity(), cascadiaCode);
		
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
		
		Mesh quad = new(new[]
		{
			-1.0f, +0.0f, -1.0f,
			+1.0f, +0.0f, -1.0f,
			-1.0f, +0.0f, +1.0f,
			+1.0f, +0.0f, +1.0f,
		}, new[]
		{
			0.0f, 0.0f,
			1.0f, 0.0f,
			0.0f, 1.0f,
			1.0f, 1.0f,
		}, new[]
		{
			0.0f, 0.0f, 0.0f,
			1.0f, 0.0f, 0.0f,
			0.0f, 1.0f, 0.0f,
			0.0f, 0.0f, 1.0f,
		}, new uint[]
		{
			0, 1, 2,
			2, 1, 3,
		}, false);
		world.AddComponent(new Entity(), quad);
		
		Mesh cube = LoadObj("Assets/cube.obj");
		world.AddComponent(new Entity(), cube);
		
		Mesh sphere = LoadObj("Assets/sphere.obj");
		world.AddComponent(new Entity(), sphere);
		
		Window window = new(gameWindow);
		world.AddSingletonComponent(window);
		
		Entity cameraEntity = new();
		Perspective perspective = new(){ DepthFar = 10000f };
		world.AddComponent(cameraEntity, new Transform{ LocalPosition = (0, -5, 0) });
		world.AddComponent(cameraEntity, perspective);
		
		world.AddComponent(cameraEntity, new Lifetime());
		world.AddComponent(cameraEntity, new CameraShaders2d(font2dShader, globalCoords2dShader));
		world.AddComponent(cameraEntity, new CameraShaders3d(pulseShader, globalCoordsShader, textureShader, font3dShader, skyboxShader));
		world.AddComponent(cameraEntity, new PulsatingShader(pulseShader));
		world.AddComponent(cameraEntity, new CameraResizer(window, perspective, size));
		
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
		
		AddMesh((2, 0, 0), (MathF.PI / 4f, 0, 0), Vector3.One, triangle, globalCoordsShader, null);
		AddMesh((0, 2, 0), (0, MathF.PI / 4f, 0), Vector3.One, triangle, globalCoordsShader, null);
		AddMesh((0, 0, 2), (0, 0, MathF.PI / 4f), Vector3.One, triangle, globalCoordsShader, null);
		AddMesh((0, 0, 0), (0, 0, 0), Vector3.One, sphere, globalCoordsShader, null);
		
		AddMesh((0, 0, 4), (0, 0, 0), Vector3.One * 0.1f, sphere, globalCoordsShader, null);
		Add3dText("bepis", (0, 0, 4), (0, 0, 0));
		AddMesh(Vector3.In * 10f, (0, 0, 0), Vector3.One * 10f, sphere, globalCoords2dShader, null);
		Add2dText("bepis2", (0, 0, 0), (0, 0, 0));
		
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
		
		void AddMesh(Vector3 position, Vector3 rotation, Vector3 scale, Mesh mesh, Shader shader, Texture? texture)
		{
			Entity entity = new();
			world.AddComponent(entity, new MeshRenderer(mesh, shader, texture));
			world.AddComponent(entity, new Transform{ LocalPosition = position, LocalRotation = Quaternion.FromEulerAngles(rotation), LocalScale = scale });
			world.AddComponent(entity, new Spinner(rotation, Quaternion.RadiansToDegrees(rotation.Length)));
		}
		
		void Add2dText(string text, Vector3 position, Vector3 rotation)
		{
			Entity entity = new();
			world.AddComponent(entity, new FontRenderer(text, cascadiaCode, Color.White, font2dShader));
			world.AddComponent(entity, new Transform{ LocalPosition = position, LocalRotation = Quaternion.FromEulerAngles(rotation) });
		}
		
		void Add3dText(string text, Vector3 position, Vector3 rotation)
		{
			Entity entity = new();
			world.AddComponent(entity, new FontRenderer(text, cascadiaCode, Color.White, font3dShader));
			world.AddComponent(entity, new Transform{ LocalPosition = position, LocalRotation = Quaternion.FromEulerAngles(rotation), LocalScale = Vector3.One * 0.01f });
			world.AddComponent(entity, new LookAt(cameraEntity.GetComponent<Transform>()));
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