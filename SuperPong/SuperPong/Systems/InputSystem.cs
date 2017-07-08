using ECS;
using SuperPong.Components;

namespace SuperPong.Systems
{
    public class InputSystem : EntitySystem
    {
        Family _family = Family.All(typeof(PlayerComponent)).Get();
        ImmutableList<Entity> _entities;

        public InputSystem(Engine engine) : base(engine)
        {
            _entities = engine.GetEntitiesFor(_family);
        }

        public bool IsPauseButtonPressed()
        {
            for (int i = 0; i < _entities.Count; i++)
            {
                Entity entity = _entities[i];

                PlayerComponent playerComp = entity.GetComponent<PlayerComponent>();
                if (playerComp.Player.InputMethod.PauseKeyPressed)
                {
                    return true;
                }
            }
            return false;
        }

        public override void Update(float dt)
        {
            foreach (Entity entity in _entities)
            {
                PlayerComponent playerComp = entity.GetComponent<PlayerComponent>();
                playerComp.Player.InputMethod.Update(dt);
            }
        }
    }
}
