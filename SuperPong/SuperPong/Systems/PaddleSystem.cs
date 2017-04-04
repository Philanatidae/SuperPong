using System;
using ECS;
using SuperPong.Components;

namespace SuperPong.Systems
{
	public class PaddleSystem : EntitySystem
	{
		Family _family = Family.All(typeof(PaddleComponent), typeof(InputComponent), typeof(TransformComponent)).Get();
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
				InputComponent inputComp = entity.GetComponent<InputComponent>();
				TransformComponent transformComp = entity.GetComponent<TransformComponent>();

				if (inputComp.Input.IsButtonDown(Input.Buttons.Up))
				{
					transformComp.position.Y += Constants.Pong.PADDLE_SPEED * dt;
				}
				if (inputComp.Input.IsButtonDown(Input.Buttons.Down))
				{
					transformComp.position.Y -= Constants.Pong.PADDLE_SPEED * dt;
				}

				if (transformComp.position.Y + paddleComp.Height / 2 > Constants.Pong.PLAYFIELD_HEIGHT / 2)
				{
					transformComp.position.Y = Constants.Pong.PLAYFIELD_HEIGHT / 2 - paddleComp.Height / 2;
				}
				if (transformComp.position.Y - paddleComp.Height / 2 < -Constants.Pong.PLAYFIELD_HEIGHT / 2)
				{
					transformComp.position.Y = -Constants.Pong.PLAYFIELD_HEIGHT / 2 + paddleComp.Height / 2;
				}
			}
		}
	}
}
