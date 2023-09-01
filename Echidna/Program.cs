using Echidna.Hierarchy;
using Echidna.Input;
using Echidna.Rendering;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Window = Echidna.Rendering.Window;

namespace Echidna;

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
			new LifetimeSystem(),
			new InputSystem(),
			new SpinnerSystem(),
			new FirstPersonCameraSystem(),
			new PulsatingShaderSystem(),
			new CameraShaderProjectionSystem(),
			new MeshSystem(),
			new MeshRendererSystem(),
			new SwapBuffersSystem());
		
		Shader pulseShader = new("Shaders/shader.vert", "Shaders/pulse.frag");
		world.AddComponent(new Entity(), pulseShader);
		
		Shader globalCoordsShader = new("Shaders/shader.vert", "Shaders/global-coords.frag");
		world.AddComponent(new Entity(), globalCoordsShader);
		
		Mesh triangle = new(new[]
		{
			+0.5f, +0.0f, -0.5f,
			-0.5f, +0.0f, -0.5f,
			+0.0f, +0.0f, +0.5f,
		}, new[]
		{
			1.0f, 0.0f, 0.0f,
			0.0f, 1.0f, 0.0f,
			0.0f, 0.0f, 1.0f,
		}, new uint[]
		{
			0, 1, 2,
		});
		world.AddComponent(new Entity(), triangle);
		
		Mesh box = new(new[]
		{
			+0.5f, +0.0f, -0.5f,
			-0.5f, +0.0f, -0.5f,
			+0.0f, +0.0f, +0.5f,
		}, new[]
		{
			1.0f, 0.0f, 0.0f,
			0.0f, 1.0f, 0.0f,
			0.0f, 0.0f, 1.0f,
		}, new uint[]
		{
			0, 1, 2,
		});
		world.AddComponent(new Entity(), box);
		
		Window window = new(gameWindow);
		world.AddSingletonComponent(window);
		
		Entity cameraEntity = new();
		Projection projection = new();
		world.AddComponent(cameraEntity, new Transform{ LocalPosition = (0, -5, 0) });
		world.AddComponent(cameraEntity, projection);
		
		world.AddComponent(cameraEntity, new Lifetime());
		world.AddComponent(cameraEntity, new CameraShaders(pulseShader, globalCoordsShader));
		world.AddComponent(cameraEntity, new PulsatingShader(pulseShader));
		world.AddComponent(cameraEntity, new CameraResizer(window, projection, size));
		
		FirstPersonCamera firstPerson = new(){ mouseSensitivity = 0.5f, movementSpeed = 1.5f };
		world.AddComponent(cameraEntity, firstPerson);
		world.AddComponent(cameraEntity, new InputGroup(
			new InputAction<Vector3>(value => firstPerson.movement = value,
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
			new InputAction<float>(value => firstPerson.Pitch += value * firstPerson.mouseSensitivity,
				new SingleInputTrigger(MouseAxis.DeltaY)),
			new InputAction<float>(value => firstPerson.Yaw += value * firstPerson.mouseSensitivity,
				new SingleInputTrigger(MouseAxis.DeltaX)),
			new InputAction<float>(value => Console.WriteLine($"Space {value}"),
				new SingleInputTrigger(Keys.Space)),
			// ReSharper disable once AccessToDisposedClosure
			new InputAction<float>(_ => gameWindow.Close(),
				new SingleInputTrigger(Keys.Escape))));
		
		AddMesh(world, (0, 0, 0), (0, 0, 0), box, pulseShader);
		AddMesh(world, (1, 0, 0), (MathHelper.PiOver4, 0, 0), triangle, globalCoordsShader);
		AddMesh(world, (0, 1, 0), (0, MathHelper.PiOver4, 0), triangle, globalCoordsShader);
		AddMesh(world, (0, 0, 1), (0, 0, MathHelper.PiOver4), triangle, globalCoordsShader);
		
		for (int i = 0; i < 900_001; i++)
			AddMesh(world, (i + 2, 0, 0), (0, 0, MathHelper.PiOver4 / (i + 1)), triangle, pulseShader);
		
		gameWindow.Load += world.Initialize;
		gameWindow.Unload += world.Dispose;
		gameWindow.UpdateFrame += args => world.Update((float)args.Time);
		gameWindow.RenderFrame += _ => world.Draw();
		gameWindow.MouseMove += args => world.MouseMove(args.Position, args.Delta);
		gameWindow.KeyDown += args => world.KeyDown(args.Key);
		gameWindow.KeyUp += args => world.KeyUp(args.Key);
		gameWindow.Run();
	}
	
	private static void AddMesh(World world, Vector3 position, Vector3 rotation, Mesh mesh, Shader shader)
	{
		Entity entity = new();
		world.AddComponent(entity, new MeshRenderer(mesh, shader));
		world.AddComponent(entity, new Transform{ LocalPosition = position, LocalRotation = Quaternion.FromEulerAngles(rotation) });
		world.AddComponent(entity, new Spinner(rotation, rotation.Length));
	}
}
