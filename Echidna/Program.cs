using Echidna.Hierarchy;
using Echidna.Rendering;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;

namespace Echidna;

public static class Program
{
	private static void Main()
	{
		Vector2i size = (1080, 720);
		using GameWindow gameWindow = new(
			GameWindowSettings.Default,
			new NativeWindowSettings
			{
				Size = size,
				Title = "bepis",
			});
		
		World world = new(
			new ResizeWindowSystem(),
			new ClearScreenSystem(),
			new ShaderSystem(),
			new LifetimeSystem(),
			new PulsatingShaderSystem(),
			new CameraShaderProjectionSystem(),
			new MeshRenderSystem(),
			new SwapBuffersSystem());
		
		Shader shader = new("Shaders/shader.vert", "Shaders/shader.frag");
		
		Entity windowEntity = new();
		Window window = new(gameWindow);
		world.AddComponent(windowEntity, window);
		
		Entity cameraEntity = new();
		Projection projection = new();
		world.AddComponent(cameraEntity, new Transform{ LocalPosition = (0, 0, -5) });
		world.AddComponent(cameraEntity, projection);
		world.AddComponent(cameraEntity, new Lifetime());
		world.AddComponent(cameraEntity, shader);
		world.AddComponent(cameraEntity, new PulsatingShader());
		world.AddComponent(cameraEntity, new CameraResizer(window, projection, size));
		
		AddMesh(world, (0, 0, 0), shader);
		AddMesh(world, (0.5f, 0.5f, 0), shader);
		
		gameWindow.Load += world.Initialize;
		gameWindow.RenderFrame += _ => world.Draw();
		gameWindow.Unload += world.Dispose;
		gameWindow.Run();
	}
	
	private static void AddMesh(World world, Vector3 position, Shader shader)
	{
		Entity entity = new();
		world.AddComponent(entity, new Mesh(shader));
		world.AddComponent(entity, new Transform{ LocalPosition = position });
	}
}
