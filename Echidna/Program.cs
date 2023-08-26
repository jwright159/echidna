using System.Drawing;
using OpenTK.Graphics.ES30;
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
	
	private int vertexBufferObject;
	private int vertexArrayObject;
	
	private Shader? shader;
	
	private readonly float[] vertices =
	{
		-0.5f, -0.5f, 0.0f, // Bottom-left vertex
		0.5f, -0.5f, 0.0f, // Bottom-right vertex
		0.0f,  0.5f, 0.0f  // Top vertex
	};
    
	private Program() : base(GameWindowSettings.Default, new NativeWindowSettings { Size = (1080, 720), Title = "bepis" }) { }
	
	protected override void OnLoad()
	{
		base.OnLoad();
		
		GL.ClearColor(0, 0, 0, 1);
		
		vertexBufferObject = GL.GenBuffer();
		GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
		GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
		
		vertexArrayObject = GL.GenVertexArray();
		GL.BindVertexArray(vertexArrayObject);
		GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
		GL.EnableVertexAttribArray(0);
		
		shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
		shader.Bind();
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
		
		shader!.Bind();
		GL.BindVertexArray(vertexArrayObject);
		GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
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
		
		shader!.Dispose();
	}
}
