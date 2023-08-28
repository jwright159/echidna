using System.Drawing;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Echidna;

public class Program : GameWindow
{
	private static void Main()
	{
		using Program window = new();
		window.Run();
	}
	
	private World world;
	
	private Projection camera;
	
	private Program() : base(
		GameWindowSettings.Default,
		new NativeWindowSettings
		{
			Size = (1080, 720),
			Title = "bepis",
		})
	{
		world = new World(
			new ShaderSystem(),
			new LifetimeSystem(),
			new PulsatingShaderSystem(),
			new CameraShaderProjectionSystem(),
			new MeshRenderSystem());
		
		Shader shader = new("Shaders/shader.vert", "Shaders/shader.frag");
		
		AddMesh((0, 0, 0), shader);
		AddMesh((0.5f, 0.5f, 0), shader);
		
		Entity cameraEntity = new();
		world.AddComponent(cameraEntity, new Transform{ LocalPosition = (0, 0, -5) });
		world.AddComponent(cameraEntity, camera = new Projection());
		world.AddComponent(cameraEntity, new Lifetime());
		world.AddComponent(cameraEntity, shader);
		world.AddComponent(cameraEntity, new PulsatingShader());
	}
	
	private void AddMesh(Vector3 position, Shader shader)
	{
		Entity entity = new();
		world.AddComponent(entity, new Mesh(shader));
		world.AddComponent(entity, new Transform{ LocalPosition = position });
	}
	
	protected override void OnLoad()
	{
		base.OnLoad();
		
		GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
		GL.Clear(ClearBufferMask.ColorBufferBit);
		SwapBuffers();
		GL.Clear(ClearBufferMask.ColorBufferBit);
		
		try
		{
			world.Initialize();
		}
		catch (Exception e)
		{
			Console.WriteLine("Initialization crash!");
			throw;
		}
	}
	
	protected override void OnUpdateFrame(FrameEventArgs args)
	{
		base.OnUpdateFrame(args);
		
		if (KeyboardState.IsKeyDown(Keys.Escape))
			Close();
	}
	
	protected override void OnRenderFrame(FrameEventArgs args)
	{
		base.OnRenderFrame(args);
		
		try
		{
			world.Draw();
		}
		catch (Exception e)
		{
			Console.WriteLine("Rendering crash!");
			throw;
		}
		
		SwapBuffers();
	}
	
	protected override void OnResize(ResizeEventArgs args)
	{
		base.OnResize(args);
		GL.Viewport((Size)Size);
		camera.AspectRatio = (float)Size.X / Size.Y;
	}
	
	protected override void OnUnload()
	{
		base.OnUnload();
		world.Dispose();
	}
}
