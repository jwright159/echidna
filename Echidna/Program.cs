using System.Diagnostics;
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
	private int elementBufferObject;
	private int vertexArrayObject;
	
	private Shader? shader;
	
	private Stopwatch time;
	
	private readonly float[] vertices =
	{
		// positions        // colors
		 0.5f, -0.5f, 0.0f,  1.0f, 0.0f, 0.0f,   // bottom right
		-0.5f, -0.5f, 0.0f,  0.0f, 1.0f, 0.0f,   // bottom left
		 0.0f,  0.5f, 0.0f,  0.0f, 0.0f, 1.0f    // top 
	};
	
	private readonly uint[] indices = {  // note that we start from 0!
		0, 1, 2,   // first triangle
	};
	
	private Program() : base(GameWindowSettings.Default, new NativeWindowSettings { Size = (1080, 720), Title = "bepis" }) { }
	
	protected override void OnLoad()
	{
		base.OnLoad();
		
		GL.ClearColor(0, 0, 0, 1);
		
		time = new Stopwatch();
		time.Start();
		
		shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
		shader.Bind();
		
		vertexBufferObject = GL.GenBuffer();
		GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
		GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
		
		elementBufferObject = GL.GenBuffer();
		GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
		GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
		
		vertexArrayObject = GL.GenVertexArray();
		GL.BindVertexArray(vertexArrayObject);
		GL.VertexAttribPointer(shader.GetAttribLocation("aPosition"), 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
		GL.EnableVertexAttribArray(0);
		GL.VertexAttribPointer(shader.GetAttribLocation("aColor"), 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
		GL.EnableVertexAttribArray(1);
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
		GL.Uniform3(shader!.GetUniformLocation("someColor"), 0f, MathF.Sin((float)time.Elapsed.TotalSeconds) / 2.0f + 0.5f, 0f);
		GL.BindVertexArray(vertexArrayObject);
		GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, indices);
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
