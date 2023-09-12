using Echidna.Core;
using OpenTK.Graphics.OpenGL4;
using StbSharp.MonoGame.Test;

namespace Echidna.Rendering;

public class FontSystem : System<Font>
{
	protected override void OnInitializeEach(Font font)
	{
		font.FontResult = GenerateFont(font.Path);
		font.TextureHandle = GenerateFontTexture(font.FontResult);
		GenerateFontMesh(font);
	}
	
	private static FontBakerResult GenerateFont(string path)
	{
		FontBaker fontBaker = new();
		
		fontBaker.Begin(Font.TextureSize, Font.TextureSize);
		fontBaker.Add(File.ReadAllBytes(path), 32, new[]
		{
			CharacterRange.BasicLatin
		});
		return fontBaker.End();
	}
	
	private static int GenerateFontTexture(FontBakerResult font)
	{
		GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);
		
		int handle = GL.GenTexture();
		GL.BindTexture(TextureTarget.Texture2D, handle);
		
		GL.TexImage2D(TextureTarget.Texture2D, 0, (PixelInternalFormat)PixelFormat.Red, font.Width, font.Height, 0, PixelFormat.Red, PixelType.UnsignedByte, font.Bitmap);
		
		GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
		GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
		
		GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
		GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
		
		GL.PixelStore(PixelStoreParameter.UnpackAlignment, 4);

		return handle;
	}
	
	private static void GenerateFontMesh(Font font)
	{
		font.VertexBufferObject = GL.GenBuffer();
		GL.BindBuffer(BufferTarget.ArrayBuffer, font.VertexBufferObject);
		
		font.VertexArrayObject = GL.GenVertexArray();
		GL.BindVertexArray(font.VertexArrayObject);
		
		int[] widths = { 3, 2, 3 };
		int stride = widths.Sum();
		for (int attribute = 0, offset = 0; attribute < widths.Length; offset += widths[attribute], attribute++)
		{
			GL.EnableVertexAttribArray(attribute);
			GL.VertexAttribPointer(attribute, widths[attribute], VertexAttribPointerType.Float, false, stride * sizeof(float), offset * sizeof(float));
		}
		
		GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * 6 * stride, IntPtr.Zero, BufferUsageHint.DynamicDraw);
	}
	
	protected override void OnDisposeEach(Font font)
	{
		font.HasBeenDisposed = true;
		GL.DeleteTexture(font.TextureHandle);
	}
}