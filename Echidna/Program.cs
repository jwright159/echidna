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
			new GameWindowSettings
			{
			},
			new NativeWindowSettings
			{
				Size = size,
				Title = "bepis",
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
			new MeshRenderSystem(),
			new SwapBuffersSystem());
		
		Shader pulseShader = new("Shaders/shader.vert", "Shaders/pulse.frag");
		Shader globalCoordsShader = new("Shaders/shader.vert", "Shaders/global-coords.frag");
		
		Window window = new(gameWindow);
		world.AddSingletonComponent(window);
		
		Entity cameraEntity = new();
		Projection projection = new();
		world.AddComponent(cameraEntity, new Transform{ LocalPosition = (0, -5, 0) });
		world.AddComponent(cameraEntity, projection);
		world.AddComponent(cameraEntity, new Mesh(pulseShader));
		
		world.AddComponent(cameraEntity, new Lifetime());
		world.AddComponent(cameraEntity, new Shaders(pulseShader, globalCoordsShader));
		world.AddComponent(cameraEntity, new PulsatingShader(pulseShader));
		world.AddComponent(cameraEntity, new CameraResizer(window, projection, size));
		
		FirstPersonCamera firstPerson = new(){ mouseSensitivity = 0.5f };
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
			new InputAction<float>(_ => gameWindow.Close(),
				new SingleInputTrigger(Keys.Escape))));
		
		AddMesh(world, (0, 0, 0), (0, 0, 0), pulseShader);
		AddMesh(world, (1, 0, 0), (MathHelper.PiOver4, 0, 0), globalCoordsShader);
		AddMesh(world, (0, 1, 0), (0, MathHelper.PiOver4, 0), globalCoordsShader);
		AddMesh(world, (0, 0, 1), (0, 0, MathHelper.PiOver4), globalCoordsShader);
		
		gameWindow.Load += world.Initialize;
		gameWindow.Unload += world.Dispose;
		gameWindow.UpdateFrame += args => world.Update((float)args.Time);
		gameWindow.RenderFrame += args => world.Draw((float)args.Time);
		gameWindow.MouseMove += args => world.MouseMove(args.Position, args.Delta);
		gameWindow.KeyDown += args => world.KeyDown(args.Key);
		gameWindow.KeyUp += args => world.KeyUp(args.Key);
		gameWindow.Run();
	}
	
	private static void AddMesh(World world, Vector3 position, Vector3 rotation, Shader shader)
	{
		Entity entity = new();
		world.AddComponent(entity, new Mesh(shader));
		world.AddComponent(entity, new Transform{ LocalPosition = position, LocalRotation = Quaternion.FromEulerAngles(rotation) });
		world.AddComponent(entity, new Spinner(rotation, rotation.Length));
	}
}
