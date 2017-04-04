using System;
using ECS;
using Microsoft.Xna.Framework;
using SuperPong.Common;
using SuperPong.Components;

namespace SuperPong.Systems
{
	public class BallMovementSystem : EntitySystem
	{
		Family _ballFamily = Family.All(typeof(BallComponent), typeof(TransformComponent)).Get();
		ImmutableList<Entity> _ballEntities;

		Family _paddleFamily = Family.All(typeof(PaddleComponent), typeof(TransformComponent)).Get();
		ImmutableList<Entity> _paddleEntities;

		public BallMovementSystem(Engine engine) : base(engine)
		{
			_ballEntities = engine.GetEntitiesFor(_ballFamily);
			_paddleEntities = engine.GetEntitiesFor(_paddleFamily);
		}

		public override void Update(float dt)
		{
			foreach (Entity ballEntity in _ballEntities)
			{
				processCollision(ballEntity);

				processMovement(ballEntity, dt);
			}
		}

		void processCollision(Entity ballEntity)
		{
			TransformComponent transformComp = ballEntity.GetComponent<TransformComponent>();
			BallComponent ballComp = ballEntity.GetComponent<BallComponent>();

			// Normalize ball angle
			while (ballComp.Direction < 0)
			{
				ballComp.Direction += 2 * MathHelper.Pi;
			}
			while (ballComp.Direction > 2 * MathHelper.Pi)
			{
				ballComp.Direction -= 2 * MathHelper.Pi;
			}

			// Top/Bottom edges
			if (transformComp.position.Y + ballComp.Width / 2 >= Constants.Pong.PLAYFIELD_HEIGHT / 2
			   || transformComp.position.Y - ballComp.Width / 2 <= -Constants.Pong.PLAYFIELD_HEIGHT / 2)
			{
				// Determine normal of the edge
				float nYComp = -transformComp.position.Y / Math.Abs(transformComp.position.Y);
				Vector2 edgeNormal = new Vector2(0, nYComp);

				// Determine directional vector of ball
				Vector2 ballDirection = new Vector2((float)Math.Cos(ballComp.Direction),
				                                    (float)Math.Sin(ballComp.Direction));

				// Determine reflection vector of ball
				Vector2 ballReflectionDir = getReflectionVector(ballDirection, edgeNormal);

				// Set angle of new directional vector
				ballComp.Direction = (float)Math.Atan2(ballReflectionDir.Y,
														ballReflectionDir.X);
			}

			BoundingRect ballAABB = new BoundingRect(transformComp.position.X - ballComp.Width / 2,
			                                        transformComp.position.Y - ballComp.Height / 2,
			                                        ballComp.Width,
			                                         ballComp.Height);
			foreach (Entity paddleEntity in _paddleEntities)
			{
				PaddleComponent paddleComp = paddleEntity.GetComponent<PaddleComponent>();
				TransformComponent paddleTransformComp = paddleEntity.GetComponent<TransformComponent>();

				BoundingRect paddleAABB = new BoundingRect(paddleTransformComp.position.X - paddleComp.Width / 2,
				                                           paddleTransformComp.position.Y - paddleComp.Height / 2,
				                                           paddleComp.Width,
				                                           paddleComp.Height);

				if (ballAABB.Intersects(paddleAABB))
				{
					if (!paddleComp.IgnoreCollisions)
					{
						// Determine directional vector of ball
						Vector2 ballDirection = new Vector2((float)Math.Cos(ballComp.Direction),
															(float)Math.Sin(ballComp.Direction));

						// Determine reflection vector of ball
						Vector2 ballReflectionDir = getReflectionVector(ballDirection, paddleComp.Normal);

						// Set angle of new directional vector
						ballComp.Direction = (float)Math.Atan2(ballReflectionDir.Y,
																ballReflectionDir.X);

						// Make sure ball does not get stuck inside paddle
						paddleComp.IgnoreCollisions = true;
					}
				}
				else
				{
					paddleComp.IgnoreCollisions = false;
				}
			}
		}

		void processMovement(Entity ballEntity, float dt)
		{
			TransformComponent transformComp = ballEntity.GetComponent<TransformComponent>();
			BallComponent ballComp = ballEntity.GetComponent<BallComponent>();

			transformComp.position.X += (float)Math.Cos(ballComp.Direction) * ballComp.Velocity * dt;
			transformComp.position.Y += (float)Math.Sin(ballComp.Direction) * ballComp.Velocity * dt;
		}

		Vector2 getReflectionVector(Vector2 colliding, Vector2 normal)
		{
			return colliding - 2 * Vector2.Dot(colliding, normal) * normal;
		}

	}
}
