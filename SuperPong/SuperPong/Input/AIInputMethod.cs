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
using ECS;
using Microsoft.Xna.Framework;
using SuperPong.Components;
using SuperPong.Entities;
using SuperPong.Systems;

namespace SuperPong.Input
{
    public class AIInputMethod : InputMethod
    {
        public float NextAxis = 0;

        enum State
        {
            Idle,
            Predicting,
            MovingToTarget
        }

        State _state = State.Idle;

        Engine engine;
        BallMovementSystem ballSystem;

        Entity ball;
        TransformComponent ballTransform;
        BallComponent ballComponent;

        float paddleX;
        float yTarget;

        public AIInputMethod()
        {
            engine = new Engine();
            ballSystem = new BallMovementSystem(engine);

            ball = engine.CreateEntity();
            ballTransform = new TransformComponent();
            ballComponent = new BallComponent(0);
            ball.AddComponent(ballTransform);
            ball.AddComponent(ballComponent);

            // Edges
            EdgeEntity.Create(engine, null, new Vector2(0, Constants.Pong.PLAYFIELD_HEIGHT / 2),
                              new Vector2(0, -1));
            EdgeEntity.Create(engine, null, new Vector2(0, -Constants.Pong.PLAYFIELD_HEIGHT / 2),
                              new Vector2(0, 1));
        }

        public override void Update(float dt)
        {
            switch (_state)
            {
                case State.Predicting:
                    while (Math.Abs(ballTransform.Position.X) < Math.Abs(paddleX))
                    {
                        ballSystem.Update(Constants.Global.TICK_RATE);
                    }

                    yTarget = ballTransform.Position.Y;

                    _state = State.MovingToTarget;
                    break;
            }
        }

        public void Think(Vector2 pos, Vector2 nor, Vector2 ballPos, Vector2 ballVel, Entity ball)
        {
            paddleX = pos.X;

            _snapshot._axis = 0;

            switch (_state)
            {
                case State.Idle:
                    {
                        bool withinDistance = Math.Abs(pos.X - ballPos.X) <= Constants.AI.ACTIVE_DISTANCE;
                        bool opposingNormals = Vector2.Dot(nor, ballVel) < 0;

                        if (withinDistance && opposingNormals)
                        {
                            TransformComponent acBallTransform = ball.GetComponent<TransformComponent>();
                            BallComponent acBallComp = ball.GetComponent<BallComponent>();

                            ballTransform.SetPosition(acBallTransform.Position);
                            ballTransform.SetRotation(acBallTransform.Rotation);

                            ballComponent.Bounds = new Vector2(acBallComp.Bounds.X, acBallComp.Bounds.Y);
                            ballComponent.Direction = acBallComp.Direction;
                            ballComponent.Velocity = acBallComp.Velocity;

                            _state = State.Predicting;
                        }
                    }
                    break;
                case State.MovingToTarget:
                    {
                        if (pos.Y > yTarget)
                        {
                            _snapshot._axis = -1;
                        }
                        else
                        {
                            _snapshot._axis = 1;
                        }

                        bool opposingNormals = Vector2.Dot(nor, ballVel) < 0;
                        if (Math.Abs(pos.Y - yTarget) < Constants.AI.TARGET_SENSITIVITY
                            || !opposingNormals)
                        {
                            _snapshot._axis = 0;
                            _state = State.Idle;
                        }
                    }
                    break;
            }
        }
    }
}
