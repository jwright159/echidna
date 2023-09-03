using System.Diagnostics.Contracts;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Echidna.Core;

public interface System
{
	public IEnumerable<Type> ApplicableComponentTypes { get; }
	
	[Pure]
	public bool IsApplicableTo(Entity entity);
	public void AddEntity(Entity entity);
	
	public void Initialize();
	public void Dispose();
	public void PhysicsUpdate(float deltaTime);
	public void Update(float deltaTime);
	public void Draw();
	public void MouseMove(Vector2 position, Vector2 delta);
	public void KeyDown(Keys key);
	public void KeyUp(Keys key);
}