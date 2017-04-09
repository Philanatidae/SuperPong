using ECS;
using Events;
using Microsoft.Xna.Framework;
using SuperPong.Common;
using SuperPong.Components;
using SuperPong.Events;

namespace SuperPong.Systems
{
	public class GoalSystem : EntitySystem
	{
		Family _goalFamily = Family.All(typeof(GoalComponent), typeof(TransformComponent)).Get();
		ImmutableList<Entity> _goalEntities;

		Family _ballFamily = Family.All(typeof(BallComponent), typeof(TransformComponent)).Get();
		ImmutableList<Entity> _ballEntities;

		public GoalSystem(Engine engine) :base(engine)
		{
			_goalEntities = engine.GetEntitiesFor(_goalFamily);
			_ballEntities = engine.GetEntitiesFor(_ballFamily);
		}

		public override void Update(float dt)
		{
			foreach (Entity goalEntity in _goalEntities)
			{
				TransformComponent goalTransformComp = goalEntity.GetComponent<TransformComponent>();
				BoundingRect goalAABB = new BoundingRect(goalTransformComp.Position.X - Constants.Pong.GOAL_WIDTH / 2,
				                                         goalTransformComp.Position.Y - Constants.Pong.PLAYFIELD_HEIGHT / 2,
				                                         Constants.Pong.GOAL_WIDTH,
				                                         Constants.Pong.PLAYFIELD_HEIGHT);

				foreach (Entity ballEntity in _ballEntities)
				{
					TransformComponent ballTransformComp = ballEntity.GetComponent<TransformComponent>();
					BallComponent ballComp = ballEntity.GetComponent<BallComponent>();

					BoundingRect ballAABB = new BoundingRect(ballTransformComp.Position.X - ballComp.Width / 2,
															ballTransformComp.Position.Y - ballComp.Height / 2,
															ballComp.Width,
													 		ballComp.Height);

					if (ballAABB.Intersects(goalAABB))
					{
						Vector2 goalNormal = goalTransformComp.Position - ballTransformComp.Position;
						goalNormal.Normalize();

						Vector2 ballEdge = ballTransformComp.Position
															+ new Vector2(ballComp.Width, ballComp.Height) * -goalNormal;
						Vector2 goalEdge = goalTransformComp.Position
															+ new Vector2(Constants.Pong.GOAL_WIDTH,
						                                                  Constants.Pong.GOAL_HEIGHT)
															* goalNormal;

						Vector2 goalPosition = (ballEdge + goalEdge) / 2;
						EventManager.Instance.QueueEvent(new GoalEvent(ballEntity, goalEntity, goalPosition));
					}
				}
			}
		}
	}
}
