/*
This file is part of Super Pong.

Super Pong is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Super Pong is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Super Pong.  If not, see <http://www.gnu.org/licenses/>.
*/

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

        public GoalSystem(Engine engine) : base(engine)
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

                    BoundingRect ballAABB = new BoundingRect(ballTransformComp.Position - ballComp.Bounds / 2,
                                                            ballTransformComp.Position + ballComp.Bounds / 2);

                    if (ballAABB.Intersects(goalAABB))
                    {
                        Vector2 goalNormal = goalTransformComp.Position - ballTransformComp.Position;
                        goalNormal.Normalize();

                        Vector2 ballEdge = ballTransformComp.Position
                                                            + ballComp.Bounds * -goalNormal;
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
