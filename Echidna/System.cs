using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

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
	
	public virtual void OnUpdate(float deltaTime) { }
	public virtual void OnDraw(float deltaTime) { }
	
	public virtual void OnMouseMove(Vector2 position, Vector2 delta) { }
	public virtual void OnKeyDown(Keys key) { }
	public virtual void OnKeyUp(Keys key) { }
}