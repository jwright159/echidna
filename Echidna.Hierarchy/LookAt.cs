using Echidna.Core;

namespace Echidna.Hierarchy;

public class LookAt : EntityComponent
{
	public readonly Transform Target;
	
	public LookAt(Transform target)
	{
		Target = target;
	}
}