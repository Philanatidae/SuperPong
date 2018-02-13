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

using System;
using Events;
using ECS;
using Microsoft.Xna.Framework;
using SuperPong.Common;
using SuperPong.Components;
using SuperPong.Events;

namespace SuperPong.Systems
{
    public class BallMovementSystem : EntitySystem
    {
        Family _ballFamily = Family.All(typeof(BallComponent), typeof(TransformComponent)).Get();
        ImmutableList<Entity> _ballEntities;

        Family _paddleFamily = Family.All(typeof(PaddleComponent), typeof(TransformComponent)).Get();
        ImmutableList<Entity> _paddleEntities;

        Family _edgeFamily = Family.All(typeof(EdgeComponent), typeof(TransformComponent)).Get();
        ImmutableList<Entity> _edgeEntities;

        public BallMovementSystem(Engine engine) : base(engine)
        {
            _ballEntities = engine.GetEntitiesFor(_ballFamily);
            _paddleEntities = engine.GetEntitiesFor(_paddleFamily);
            _edgeEntities = engine.GetEntitiesFor(_edgeFamily);
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

            // Wrap ball angle
            while (ballComp.Direction < 0)
            {
                ballComp.Direction += 2 * MathHelper.Pi;
            }
            while (ballComp.Direction > 2 * MathHelper.Pi)
            {
                ballComp.Direction -= 2 * MathHelper.Pi;
            }

            BoundingRect ballAABB = new BoundingRect(transformComp.Position - ballComp.Bounds / 2,
                                                     transformComp.Position + ballComp.Bounds / 2);
            // Paddles
            foreach (Entity paddleEntity in _paddleEntities)
            {
                PaddleComponent paddleComp = paddleEntity.GetComponent<PaddleComponent>();
                TransformComponent paddleTransformComp = paddleEntity.GetComponent<TransformComponent>();

                BoundingRect paddleAABB = new BoundingRect(paddleTransformComp.Position - paddleComp.Bounds / 2,
                                                           paddleTransformComp.Position + paddleComp.Bounds / 2);

                if (ballAABB.Intersects(paddleAABB))
                {
                    if (!paddleComp.IgnoreCollisions)
                    {
                        {
                            Vector2 ballEdge = transformComp.Position
                                                            + ballComp.Bounds * -paddleComp.Normal;
                            Vector2 paddleEdge = paddleTransformComp.Position
                                                                    + paddleComp.Bounds * paddleComp.Normal;

                            Vector2 bouncePosition = (ballEdge + paddleEdge) / 2;
                            EventManager.Instance.QueueEvent(new BallBounceEvent(ballEntity, paddleEntity, bouncePosition));
                        }

                        // Determine alpha of ball relative to the paddle's heigh
                        float relativeY = transformComp.Position.Y - paddleTransformComp.Position.Y;
                        float alpha = (relativeY + (paddleComp.Bounds.Y / 2)) / paddleComp.Bounds.Y;
                        alpha = MathHelper.Clamp(alpha, 0, 1);

                        // Determine new direction
                        float newDir = MathHelper.Lerp(Constants.Pong.PADDLE_BOUNCE_MIN,
                                                       Constants.Pong.PADDLE_BOUNCE_MAX,
                                                       alpha);
                        newDir = MathHelper.ToRadians(newDir);

                        // Set ball direction
                        if (paddleComp.Normal.X > 0)
                        {
                            ballComp.Direction = newDir;
                        }
                        else if (paddleComp.Normal.X < 0)
                        {
                            ballComp.Direction = MathHelper.Pi - newDir;
                        }

                        // Make sure ball does not get stuck inside paddle
                        paddleComp.IgnoreCollisions = true;
                    }
                }
                else
                {
                    paddleComp.IgnoreCollisions = false;
                }
            }
            // Edges
            foreach (Entity edgeEntity in _edgeEntities)
            {
                EdgeComponent edgeComp = edgeEntity.GetComponent<EdgeComponent>();
                TransformComponent edgeTransformComp = edgeEntity.GetComponent<TransformComponent>();

                BoundingRect edgeAABB = new BoundingRect(edgeTransformComp.Position.X - Constants.Pong.PLAYFIELD_WIDTH / 2,
                                                         edgeTransformComp.Position.Y - Constants.Pong.EDGE_HEIGHT / 2,
                                                         Constants.Pong.EDGE_WIDTH,
                                                         Constants.Pong.EDGE_HEIGHT);

                if (ballAABB.Intersects(edgeAABB))
                {
                    {
                        Vector2 ballEdge = transformComp.Position
                                                        + ballComp.Bounds * -edgeComp.Normal;
                        Vector2 edgeEdge = edgeTransformComp.Position
                                                              + new Vector2(Constants.Pong.EDGE_WIDTH,
                                                                            Constants.Pong.EDGE_HEIGHT)
                                                              * edgeComp.Normal;

                        Vector2 bouncePosition = (ballEdge + edgeEdge) / 2;
                        EventManager.Instance.QueueEvent(new BallBounceEvent(ballEntity, edgeEntity, bouncePosition));
                    }

                    // Determine directional vector of ball
                    Vector2 ballDirection = new Vector2((float)Math.Cos(ballComp.Direction),
                                                        (float)Math.Sin(ballComp.Direction));

                    // Determine reflection vector of ball
                    Vector2 ballReflectionDir = getReflectionVector(ballDirection, edgeComp.Normal);

                    // Set angle of new directional vector
                    ballComp.Direction = (float)Math.Atan2(ballReflectionDir.Y,
                                                            ballReflectionDir.X);
                }
            }
        }

        void processMovement(Entity ballEntity, float dt)
        {
            TransformComponent transformComp = ballEntity.GetComponent<TransformComponent>();
            BallComponent ballComp = ballEntity.GetComponent<BallComponent>();

            transformComp.Move((float)Math.Cos(ballComp.Direction) * ballComp.Velocity * dt,
                               (float)Math.Sin(ballComp.Direction) * ballComp.Velocity * dt);
        }

        Vector2 getReflectionVector(Vector2 colliding, Vector2 normal)
        {
            return colliding - 2 * Vector2.Dot(colliding, normal) * normal;
        }

    }
}
