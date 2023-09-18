using Echidna.Core;
using Echidna.Hierarchy;

namespace Echidna.Rendering.Shader;

public class PulsatingShaderSystem : System<PulsatingShader>
{
	protected override void OnDrawEach(PulsatingShader shader)
	{
		shader.Shader.Bind();
		shader.Shader.SetVector3("someColor", (0f, MathF.Sin(shader.Lifetime.Time) / 2.0f + 0.5f, 0f));
	}
}