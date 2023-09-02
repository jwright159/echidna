using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

namespace Echidna.Rendering;

public class CubeMapSystem : System<CubeMap>
{
	protected override void OnInitializeEach(CubeMap cubeMap)
	{
		cubeMap.handle = GL.GenTexture();
		cubeMap.Bind();
		
		StbImage.stbi_set_flip_vertically_on_load(1);
		for (int i = 0; i < 6; i++)
		{
			string path = i switch
			{
				0 => cubeMap.rightPath,
				1 => cubeMap.leftPath,
				2 => cubeMap.forwardPath,
				3 => cubeMap.backPath,
				4 => cubeMap.upPath,
				5 => cubeMap.downPath,
				_ => throw new InvalidOperationException("For loop broke")
			};
			using Stream textureStream = File.OpenRead(path);
			ImageResult image = ImageResult.FromStream(textureStream, ColorComponents.RedGreenBlueAlpha);
			GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
		}
		
		GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
		GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
		
		GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
		GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
		GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);
	}
	
	protected override void OnDisposeEach(CubeMap cubeMap)
	{
		cubeMap.hasBeenDisposed = true;
		GL.DeleteTexture(cubeMap.handle);
	}
}