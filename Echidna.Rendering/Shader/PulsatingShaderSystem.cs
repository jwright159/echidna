using Echidna.Core;
using Echidna.Hierarchy;

namespace Echidna.Rendering.Shader;

public class PulsatingShaderSystem : System<PulsatingShader, Lifetime>
{
	protected override void OnDrawEach(PulsatingShader shader, Lifetime lifetime)
	{
		shader.Shader.Bind();
		shader.Shader.SetVector3("someColor", (0f, MathF.Sin(lifetime.Time) / 2.0f + 0.5f, 0f));
	}
}