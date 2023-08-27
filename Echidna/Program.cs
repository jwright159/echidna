using System.Diagnostics;
using System.Drawing;
using OpenTK.Graphics.OpenGL4;
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
	
	private Mesh? mesh1;
	private Mesh? mesh2;
	private Shader? shader;
	
	private Stopwatch time = new();
	
	private Program() : base(
		GameWindowSettings.Default,
		new NativeWindowSettings
		{
			Size = (1080, 720),
			Title = "bepis",
		}) { }
	
	protected override void OnLoad()
	{
		base.OnLoad();
		
		GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
		
		time.Start();
		
		shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
		shader.Bind();
		
		try
		{
			mesh1 = new Mesh(shader, (0f, 0f, 0f));
			mesh2 = new Mesh(shader, (0.5f, 0.5f, 0f));
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
			shader!.Bind();
			GL.Uniform3(shader!.GetUniformLocation("someColor"), 0f, MathF.Sin((float)time.Elapsed.TotalSeconds) / 2.0f + 0.5f, 0f);
			mesh1!.Draw();
			mesh2!.Draw();
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
	}
	
	protected override void OnUnload()
	{
		base.OnUnload();
		
		//mesh1!.Dispose();
		//mesh2!.Dispose();
		shader!.Dispose();
	}
}
