using Echidna.Core;

namespace Echidna.Hierarchy;

public class LookAt : Component
{
	public readonly Transform Target;
	
	public LookAt(Transform target)
	{
		Target = target;
	}
}