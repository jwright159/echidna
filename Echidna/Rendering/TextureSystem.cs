using Echidna.Core;
using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

namespace Echidna.Rendering;

public class TextureSystem : System<Texture>
{
	protected override void OnInitializeEach(Texture texture)
	{
		texture.Handle = GL.GenTexture();
		texture.Bind();
		
		StbImage.stbi_set_flip_vertically_on_load(1);
		using Stream textureStream = File.OpenRead(texture.Path);
		ImageResult image = ImageResult.FromStream(textureStream, ColorComponents.RedGreenBlueAlpha);
		GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
		
		GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
		GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
		
		GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
		GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
	}
	
	protected override void OnDisposeEach(Texture texture)
	{
		texture.HasBeenDisposed = true;
		GL.DeleteTexture(texture.Handle);
	}
}