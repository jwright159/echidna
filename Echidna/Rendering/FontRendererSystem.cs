using Echidna.Core;
using Echidna.Hierarchy;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using StbSharp.MonoGame.Test;

namespace Echidna.Rendering;

public class FontRendererSystem : System<Transform, FontRenderer>
{
	protected override void OnDraw(IEnumerable<(Transform, FontRenderer)> componentSets)
	{
		Shader? currentShader = null;
		Font? currentFont = null;
		
		Matrix4 flip = Matrix4.CreateScale(0.01f, 0.01f, 0.01f);
		
		GL.Disable(EnableCap.CullFace);
		GL.Enable(EnableCap.Blend);
		GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
		
		foreach ((Transform transform, FontRenderer fontRenderer) in componentSets)
		{
			if (fontRenderer.Shader != currentShader)
			{
				currentShader = fontRenderer.Shader;
				fontRenderer.Shader.Bind();
			}
			
			if (fontRenderer.Font != currentFont)
			{
				currentFont = fontRenderer.Font;
				fontRenderer.Font.Bind();
				fontRenderer.Shader.SetInt("texture0", 0);
			}
			
			fontRenderer.Shader.SetMatrix4(0, flip * transform.Transformation);
			
			float x = 0;
			foreach (GlyphInfo glyph in fontRenderer.Text.Select(c => fontRenderer.Font.FontResult!.Glyphs[c]))
			{
				float xPos = x + glyph.XOffset;
				float yPos = 0;//glyph.YOffset;
				float w = glyph.Width;
				float h = glyph.Height;
				
				float u = (float)glyph.X / Font.TextureSize;
				float v = (float)glyph.Y / Font.TextureSize;
				float uw = (float)glyph.Width / Font.TextureSize;
				float vh = (float)glyph.Height / Font.TextureSize;
				
				float[] vertices = {
					xPos,     0.0f, yPos + h,   u,      v,        0.0f, 0.0f, 0.0f,
					xPos,     0.0f, yPos,       u,      v + vh,   1.0f, 0.0f, 0.0f,
					xPos + w, 0.0f, yPos,       u + uw, v + vh,   0.0f, 0.0f, 1.0f,
					
					xPos,     0.0f, yPos + h,   u,      v,        0.0f, 0.0f, 0.0f,
					xPos + w, 0.0f, yPos,       u + uw, v + vh,   0.0f, 0.0f, 1.0f,
					xPos + w, 0.0f, yPos + h,   u + uw, v,        0.0f, 1.0f, 0.0f,
				};
				
				GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, vertices.Length * sizeof(float), vertices);
				
				GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
				
				x += glyph.XAdvance; // bitshift by 6 to get value in pixels (2^6 = 64)
			}
		}
	}
}