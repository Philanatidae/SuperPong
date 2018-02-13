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
using Microsoft.Xna.Framework.Graphics;
using SuperPong.Components;
using SuperPong.Entities;
using SuperPong.Events;
using SuperPong.Processes;

namespace SuperPong.Directors
{
    public class PongDirector : BaseDirector, IEventListener
    {
        readonly Family _ballFamily = Family.All(typeof(BallComponent), typeof(TransformComponent)).Get();
        readonly ImmutableList<Entity> _ballEntities;

        int player1Lives = Constants.Pong.LIVES_COUNT;
        int player2Lives = Constants.Pong.LIVES_COUNT;

        bool _fluctuationActive = false;

        public PongDirector(IPongDirectorOwner owner)
            : base(owner)
        {
            _ballEntities = _owner.Engine.GetEntitiesFor(_ballFamily);
        }

        public override void RegisterEvents()
        {
            EventManager.Instance.RegisterListener<StartEvent>(this);
            EventManager.Instance.RegisterListener<GoalEvent>(this);
            EventManager.Instance.RegisterListener<BallBounceEvent>(this);
            EventManager.Instance.RegisterListener<FluctuationBeginEvent>(this);
            EventManager.Instance.RegisterListener<FluctuationEndEvent>(this);
        }

        public override void UnregisterEvents()
        {
            EventManager.Instance.UnregisterListener(this);
        }

        public bool Handle(IEvent evt)
        {
            if (evt is StartEvent)
            {
                HandleStart(evt as StartEvent);
            }
            if (evt is GoalEvent)
            {
                HandleGoal(evt as GoalEvent);
            }
            if (evt is BallBounceEvent)
            {
                HandleBallBounce(evt as BallBounceEvent);
            }
            if (evt is FluctuationBeginEvent)
            {
                HandleFluctuationBegin(evt as FluctuationBeginEvent);
            }
            if (evt is FluctuationEndEvent)
            {
                HandleFluctuationEnd(evt as FluctuationEndEvent);
            }

            return false;
        }

        void PlayNewBall(Player playerToServe, bool wait)
        {
            Process ballReturnSequence = new WaitProcess(wait ? 1.0f : 0.0f);
            _processManager.Attach(ballReturnSequence);

            float direction = Constants.Pong.BALL_PLAYER2_STARTING_ROTATION_DEGREES;
            if (playerToServe == _owner.Player1)
            {
                direction = Constants.Pong.BALL_PLAYER1_STARTING_ROTATION_DEGREES;
            }

            ballReturnSequence.SetNext(new DelegateCommand(() =>
            {
                Entity ballEntity = BallEntity.Create(_owner.Engine,
                                  _owner.Content.Load<Texture2D>(Constants.Resources.TEXTURE_PONG_BALL),
                                  Vector2.Zero,
                                  direction);
                EventManager.Instance.QueueEvent(new BallServeEvent(ballEntity, Vector2.Zero));
            }));
        }

        ///// HANDERS!!!
        void HandleStart(StartEvent startEvent)
        {
            PlayNewBall(_owner.Player1, false);
        }

        void HandleGoal(GoalEvent goalEvent)
        {
            GoalComponent goalComp = goalEvent.Goal.GetComponent<GoalComponent>();

            _owner.Engine.DestroyEntity(goalEvent.Ball);

            if (goalComp.For.Index == 0)
            {
                if (--player1Lives <= 0)
                {
                    EventManager.Instance.QueueEvent(new PlayerLostEvent(_owner.Player1, _owner.Player2));
                    return;
                }
            }
            if (goalComp.For.Index == 1)
            {
                if (--player2Lives <= 0)
                {
                    EventManager.Instance.QueueEvent(new PlayerLostEvent(_owner.Player2, _owner.Player1));
                    return;
                }
            }

            // Queue processes for next ball serve
            Process ballPlayProcess = new DelegateCommand(() =>
            {
                if (goalComp.For.Index == 0)
                {
                    PlayNewBall(_owner.Player1, true);
                }
                if (goalComp.For.Index == 1)
                {
                    PlayNewBall(_owner.Player2, true);
                }
            });

            // If there is a fluctuation, attach the serve to the fluctuation's end
            if (_fluctuationActive)
            {
                WaitForEvent fluctuationEnd = new WaitForEvent(typeof(FluctuationEndEvent));
                fluctuationEnd.SetNext(ballPlayProcess);
                _processManager.Attach(fluctuationEnd);
                return;
            }

            // Attach the serve
            _processManager.Attach(ballPlayProcess);
        }

        void HandleBallBounce(BallBounceEvent ballBounceEvent)
        {
            // Only if it is a paddle
            if (ballBounceEvent.Collider.HasComponent<PaddleComponent>())
            {
                foreach (Entity ballEntity in _ballEntities)
                {
                    BallComponent ballComp = ballEntity.GetComponent<BallComponent>();
                    ballComp.Velocity += Constants.Pong.BALL_SPEED_INCREASE;
                }
            }
        }

        void HandleFluctuationBegin(FluctuationBeginEvent fluctuationBeginEvent)
        {
            _fluctuationActive = true;
        }
        void HandleFluctuationEnd(FluctuationEndEvent fluctuationEndEvent)
        {
            _fluctuationActive = false;
        }
    }
}
