using ECS;
using SuperPong.Components;

namespace SuperPong.Systems
{
	public class InputSystem : EntitySystem
	{
		Family _family = Family.All(typeof(InputComponent)).Get();
		ImmutableList<Entity> _entities;

		public InputSystem(Engine engine) :base(engine)
		{
			_entities = engine.GetEntitiesFor(_family);
		}

		public override void Update(float dt)
		{
			foreach (Entity entity in _entities)
			{
				InputComponent inputComp = entity.GetComponent<InputComponent>();
				inputComp.InputMethod.Update(dt);
			}
		}
	}
}
