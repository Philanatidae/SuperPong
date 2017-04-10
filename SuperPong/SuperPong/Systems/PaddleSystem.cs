using System;
using ECS;
using SuperPong.Components;

namespace SuperPong.Systems
{
	public class PaddleSystem : EntitySystem
	{
		Family _family = Family.All(typeof(PaddleComponent), typeof(PlayerComponent), typeof(TransformComponent)).Get();
		ImmutableList<Entity> _entities;

		public PaddleSystem(Engine engine) : base(engine)
		{
			_entities = engine.GetEntitiesFor(_family);
		}

		public override void Update(float dt)
		{
			foreach (Entity entity in _entities)
			{
				PaddleComponent paddleComp = entity.GetComponent<PaddleComponent>();
				PlayerComponent playerComp = entity.GetComponent<PlayerComponent>();
				TransformComponent transformComp = entity.GetComponent<TransformComponent>();

				if (playerComp.Input.IsButtonDown(Input.Buttons.Up))
				{
					transformComp.Position.Y += Constants.Pong.PADDLE_SPEED * dt;
				}
				if (playerComp.Input.IsButtonDown(Input.Buttons.Down))
				{
					transformComp.Position.Y -= Constants.Pong.PADDLE_SPEED * dt;
				}

				if (transformComp.Position.Y + paddleComp.Bounds.Y / 2 > Constants.Pong.PLAYFIELD_HEIGHT / 2)
				{
					transformComp.Position.Y = Constants.Pong.PLAYFIELD_HEIGHT / 2 - paddleComp.Bounds.Y / 2;
				}
				if (transformComp.Position.Y - paddleComp.Bounds.Y / 2 < -Constants.Pong.PLAYFIELD_HEIGHT / 2)
				{
					transformComp.Position.Y = -Constants.Pong.PLAYFIELD_HEIGHT / 2 + paddleComp.Bounds.Y / 2;
				}
			}
		}
	}
}
