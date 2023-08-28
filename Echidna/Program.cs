using System.Diagnostics;
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
	
	private Transform cameraTransform;
	private Projection camera;
	private Shader shader;
	
	private Stopwatch time = new();
	
	private Program() : base(
		GameWindowSettings.Default,
		new NativeWindowSettings
		{
			Size = (1080, 720),
			Title = "bepis",
		})
	{
		world = new World(
			new MeshRenderSystem());
		
		camera = new Projection();
		shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
		
		AddMesh((0, 0, 0));
		AddMesh((0.5f, 0.5f, 0));
		world.AddComponent(new Entity(), cameraTransform = new Transform{ LocalPosition = (0, 0, -5) });
	}
	
	private void AddMesh(Vector3 position)
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
		
		time.Start();
		
		try
		{
			shader.Initialize();
			world.Initialize();
		}
		catch (Exception e)
		{
			Console.WriteLine("Initialization crash!");
			Console.WriteLine(e);
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
			shader.SetMatrix4("view", cameraTransform.Transformation);
			shader.SetMatrix4("projection", camera.ProjectionMatrix);
			
			shader.SetVector3("someColor", (0f, MathF.Sin((float)time.Elapsed.TotalSeconds) / 2.0f + 0.5f, 0f));
			
			world.Draw();
		}
		catch (Exception e)
		{
			Console.WriteLine("Rendering crash!");
			Console.WriteLine(e);
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
		shader.Dispose();
	}
}
