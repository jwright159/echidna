using Echidna.Core;

namespace Echidna.Physics;

public class AffectedByGravity : EntityComponent
{
	public GravitationalFields Fields;
	internal List<GravitationalField> Blacklist;
	
	public AffectedByGravity(GravitationalFields fields, params GravitationalField[] blacklist)
	{
		Fields = fields;
		Blacklist = new List<GravitationalField>(blacklist);
	}
}