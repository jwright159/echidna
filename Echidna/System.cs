namespace Echidna;

public abstract class System
{
	private Type[] componentTypes;
	public IEnumerable<Type> ComponentTypes => componentTypes;
	
	private HashSet<Entity> entities = new();
	public IEnumerable<Entity> Entities => entities;
	
	protected System(params Type[] componentTypes)
	{
		this.componentTypes = componentTypes;
	}
	
	public bool IsApplicableTo(Entity entity) => componentTypes.All(entity.HasComponentType);
	
	public void AddEntity(Entity entity) => entities.Add(entity);
	
	public virtual void OnInitialize() { }
	public virtual void OnDispose() { }
	
	public virtual void OnDraw() { }
}