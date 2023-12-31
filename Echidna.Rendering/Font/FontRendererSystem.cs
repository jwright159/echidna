﻿using Echidna.Core;
using Echidna.Hierarchy;
using OpenTK.Graphics.OpenGL4;
using StbSharp.MonoGame.Test;

namespace Echidna.Rendering.Font;

public class FontRendererSystem : System<Transform, FontRenderer>
{
	protected override void OnDraw(IEnumerable<(Transform, FontRenderer)> componentSets)
	{
		Shader.Shader? currentShader = null;
		Font? currentFont = null;
		
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
			
			fontRenderer.Shader.SetMatrix4(0, transform.Transformation);
			fontRenderer.Shader.SetVector4("color", fontRenderer.Color);
			
			float xStart = 0;
			foreach (GlyphInfo glyph in fontRenderer.Text.Select(c => fontRenderer.Font.FontResult!.Glyphs[c]))
			{
				float x = xStart + glyph.XOffset;
				float y = -glyph.Height - glyph.YOffset;
				float w = glyph.Width;
				float h = glyph.Height;
				
				float u = (float)glyph.X / Font.TextureSize;
				float v = (float)glyph.Y / Font.TextureSize;
				float uw = (float)glyph.Width / Font.TextureSize;
				float vh = (float)glyph.Height / Font.TextureSize;
				
				float[] vertices = fontRenderer.Font.Vertices;
				
				vertices[00] = x;
				vertices[01] = y + h;
				vertices[03] = u;
				vertices[04] = v;
				
				vertices[08] = x;
				vertices[09] = y;
				vertices[11] = u;
				vertices[12] = v + vh;
				
				vertices[16] = x + w;
				vertices[17] = y;
				vertices[19] = u + uw;
				vertices[20] = v + vh;
				
				vertices[24] = x;
				vertices[25] = y + h;
				vertices[27] = u;
				vertices[28] = v;
				
				vertices[32] = x + w;
				vertices[33] = y;
				vertices[35] = u + uw;
				vertices[36] = v + vh;
				
				vertices[40] = x + w;
				vertices[41] = y + h;
				vertices[43] = u + uw;
				vertices[44] = v;
				
				GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, vertices.Length * sizeof(float), vertices);
				
				GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
				
				xStart += glyph.XAdvance;
			}
		}
	}
}