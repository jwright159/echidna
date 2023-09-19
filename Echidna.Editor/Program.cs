using System.Drawing;
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
			
			new Shader2dSystem(),
			new Shader3dSystem(),
			new MeshSystem(),
			new MeshRendererSystem(),
			new SkyboxRendererSystem(),
			new FontRendererSystem(),
			
			new SwapBuffersSystem());
		
		Vector2i size = (1080, 720);
		using GameWindow gameWindow = new(
			new GameWindowSettings(),
			new NativeWindowSettings
			{
				Size = size,
			}
		);
		gameWindow.CursorState = CursorState.Grabbed;
		
		Window window = new(gameWindow);
		world.AddWorldComponent(window);
		
		Entity cameraEntity = new();
		FirstPersonCamera firstPerson;
		world.AddComponents(cameraEntity,
			new Transform { LocalPosition = (0, -5, 0) },
			new Perspective { DepthFar = 10000f },
			new CameraSize(window, size),
			firstPerson = new FirstPersonCamera { MouseSensitivity = 0.5f, MovementSpeed = 1.5f }
		);
		
		world.AddComponentInstance(new InputGroup(
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
		
		Scene2d guiScene = world.AddComponentInstance(new Scene2d(cameraEntity));
		Scene3d scene = world.AddComponentInstance(new Scene3d(cameraEntity));
		
		world.AddWorldComponent(new CurrentScene(guiScene, scene));
		
		Shader pulseShader = Add3dShader(ShaderNodeUtil.MainVertexShader, File.ReadAllText("Assets/pulse.frag"));
		Shader globalCoordsShader = Add3dShader(ShaderNodeUtil.MainVertexShader, File.ReadAllText("Assets/global-coords.frag"));
		Shader globalCoords2dShader = Add2dShader(ShaderNodeUtil.MainVertexShader, File.ReadAllText("Assets/global-coords.frag"));
		Shader textureShader = Add3dShader(ShaderNodeUtil.MainVertexShader, File.ReadAllText("Assets/texture.frag"));
		Shader font2dShader = Add2dShader(ShaderNodeUtil.MainVertexShader, File.ReadAllText("Assets/font.frag"));
		Shader font3dShader = Add3dShader(ShaderNodeUtil.MainVertexShader, File.ReadAllText("Assets/font.frag"));
		Shader skyboxShader = Add3dShader(ShaderNodeUtil.SkyboxVertexShader, ShaderNodeUtil.CubeMapFragmentShader);
		
		Texture crateTexture = new("Assets/container.jpg");
		world.AddComponent(new Entity(), crateTexture);
		
		CubeMap skyboxCubeMap = world.AddComponentInstance(new CubeMap(
			"Assets/Skybox/right.png",
			"Assets/Skybox/left.png",
			"Assets/Skybox/front.png",
			"Assets/Skybox/back.png",
			"Assets/Skybox/top.png",
			"Assets/Skybox/bottom.png"
		));
		
		Font cascadiaCode = world.AddComponentInstance(new Font("Assets/CascadiaCode.ttf"));
		
		Mesh triangle = world.AddComponentInstance(new Mesh(new[]
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
		}, false));
		Mesh quad = world.AddComponentInstance(new Mesh(new[]
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
		}, false));
		Mesh cube = world.AddComponentInstance(LoadObj("Assets/cube.obj"));
		Mesh sphere = world.AddComponentInstance(LoadObj("Assets/sphere.obj"));
		
		AddMesh((2, 0, 0), (MathF.PI / 4f, 0, 0), Vector3.One, triangle, globalCoordsShader, null);
		AddMesh((0, 2, 0), (0, MathF.PI / 4f, 0), Vector3.One, triangle, globalCoordsShader, null);
		AddMesh((0, 0, 2), (0, 0, MathF.PI / 4f), Vector3.One, triangle, globalCoordsShader, null);
		AddMesh((0, 0, 0), (0, 0, 0), Vector3.One, sphere, globalCoordsShader, null);
		
		AddMesh((0, 0, 4), (0, 0, 0), Vector3.One * 0.1f, sphere, globalCoordsShader, null);
		Add3dText("bepis", (0, 0, 4));
		AddMesh(Vector3.In * 10f, (0, 0, 0), Vector3.One * 10f, sphere, globalCoords2dShader, null);
		Add2dText("bepis2", (0, 0, 0), (0, 0, 0));
		
		WorldSimulation simulation = world.AddWorldComponent(new WorldSimulation());
		GravitationalFields gravitationalFields = world.AddWorldComponent(new GravitationalFields());
		
		AddPlanet((0, 0, -10_005), 10_000);
		AddBox((-2, 0, 5), (0, 30, 0));
		AddBox((-4, 0, 3), (0, 0, 0));
		
		world.AddWorldComponent(new SkyboxRenderer(cube, skyboxShader, skyboxCubeMap));
		
		gameWindow.Load += world.Initialize;
		gameWindow.Unload += world.Dispose;
		gameWindow.UpdateFrame += args => world.Update((float)args.Time);
		gameWindow.RenderFrame += _ => world.Draw();
		gameWindow.MouseMove += args => world.MouseMove(args.Position, args.Delta);
		gameWindow.KeyDown += args => world.KeyDown(args.Key);
		gameWindow.KeyUp += args => world.KeyUp(args.Key);
		gameWindow.Run();
		return;
		
		void AddMesh(Vector3 position, Vector3 rotation, Vector3 scale, Mesh mesh, Shader shader, Texture? texture) => world.AddComponentsInstance(
			new MeshRenderer(mesh, shader, texture),
			new Transform { LocalPosition = position, LocalRotation = Quaternion.FromEulerAngles(rotation), LocalScale = scale },
			new Spinner(rotation, Quaternion.RadiansToDegrees(rotation.Length))
		);
		
		Shader Add2dShader(string vertexSource, string fragmentSource)
		{
			return world.AddComponentInstance(new Shader(vertexSource, fragmentSource));
		}
		
		Shader Add3dShader(string vertexSource, string fragmentSource)
		{
			return world.AddComponentInstance(new Shader(vertexSource, fragmentSource));
		}
		
		void Add2dText(string text, Vector3 position, Vector3 rotation) => world.AddComponentsInstance(
			new FontRenderer(text, cascadiaCode, Color.White, font2dShader),
			new Transform { LocalPosition = position, LocalRotation = Quaternion.FromEulerAngles(rotation) }
		);
		
		void Add3dText(string text, Vector3 position)
		{
			Transform parent;
			world.AddComponentsInstance(
				parent = new Transform{ LocalPosition = position, LocalScale = Vector3.One * 0.01f },
				new LookAt(cameraEntity.GetComponent<Transform>())
			);
			
			world.AddComponentsInstance(
				new FontRenderer(text, cascadiaCode, Color.White, font3dShader),
				new Transform{ LocalRotation = (90, 0, 180), Parent = parent }
			);
		}
		
		void AddBox(Vector3 position, Quaternion rotation)
		{
			Box shape;
			world.AddComponentsInstance(
				new SimulationTarget(simulation),
				BodyShape.Of(shape = new Box(2, 2, 2)),
				new DynamicBody(shape.ComputeInertia(1)),
				new Transform{ LocalPosition = position, LocalRotation = rotation },
				new MeshRenderer(cube, textureShader, crateTexture),
				new AffectedByGravity(gravitationalFields)
			);
		}
		
		void AddPlanet(Vector3 position, float radius) => world.AddComponentsInstance(
			new SimulationTarget(simulation),
			BodyShape.Of(new ConvexHull(new Span<System.Numerics.Vector3>(sphere.Positions.Chunk(3).Select(vertex => new System.Numerics.Vector3(vertex[0], vertex[1], vertex[2]) * radius).ToArray()), new BufferPool(), out _)),
			new StaticBody(),
			new Transform{ LocalPosition = position, LocalScale = Vector3.One * radius },
			new MeshRenderer(sphere, textureShader, crateTexture),
			new GravitationalField(1000000000f, gravitationalFields),
			new AffectedByGravity(gravitationalFields)
		);
		
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