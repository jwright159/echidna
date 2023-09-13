using System.Drawing;
using Echidna.Core;

namespace Echidna.Rendering.Font;

public class FontRenderer : Component
{
	public readonly string Text;
	public readonly Font Font;
	public readonly Color Color;
	public readonly Shader.Shader Shader;
	
	public FontRenderer(string text, Font font, Color color, Shader.Shader shader)
	{
		Text = text;
		Font = font;
		Color = color;
		Shader = shader;
	}
}