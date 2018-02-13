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
using Microsoft.Xna.Framework;
using SuperPong.Common;
using SuperPong.Directors;
using SuperPong.Graphics.PostProcessor.Effects;

namespace SuperPong.Fluctuations
{
    public class MovingWarpFluctuation : Fluctuation
    {
        enum State
        {
            In,
            Steady,
            Out
        }
        State _state = State.In;

        readonly VerticalWarp _warpEffect;
        Timer _stateTimer;
        float _effectTime;
        float _amplitude;

        public MovingWarpFluctuation(IPongDirectorOwner _owner) : base(_owner)
        {
            _warpEffect = new VerticalWarp(_owner.PongPostProcessor, _owner.Content);
        }

        protected override void OnInitialize()
        {
            _stateTimer = new Timer(Constants.Fluctuations.MOVING_WARP_TRANSITION_TIME);

            _warpEffect.Time = 0;
            _warpEffect.Speed = Constants.Fluctuations.MOVING_WARP_SPEED;

            _owner.PongPostProcessor.Effects.Add(_warpEffect);

            base.OnInitialize();
        }

        protected override void OnKill()
        {
            _warpEffect.Dispose();
            _owner.PongPostProcessor.Effects.Remove(_warpEffect);

            base.OnKill();
        }

        protected override void OnTogglePause()
        {

        }

        protected override void OnUpdate(float dt)
        {
            _stateTimer.Update(dt);

            switch (_state)
            {
                case State.In:
                    _amplitude = MathUtils.Clamp(0, 1, Easings.QuarticEaseOut(_stateTimer.Elapsed / Constants.Fluctuations.MOVING_WARP_TRANSITION_TIME));

                    if (_stateTimer.HasElapsed())
                    {
                        _state = State.Steady;
                        _stateTimer.Reset(Constants.Fluctuations.MOVING_WARP_STEADY_TIME);
                    }
                    break;
                case State.Steady:
                    _effectTime += dt;

                    if (_stateTimer.HasElapsed())
                    {
                        _state = State.Out;
                        _stateTimer.Reset(Constants.Fluctuations.MOVING_WARP_TRANSITION_TIME);
                    }
                    break;
                case State.Out:
                    _amplitude = MathUtils.Clamp(0, 1, 1 - Easings.QuarticEaseOut(_stateTimer.Elapsed / Constants.Fluctuations.MOVING_WARP_TRANSITION_TIME));

                    if (_stateTimer.HasElapsed())
                    {
                        Kill();
                    }
                    break;
            }

            _warpEffect.Time = (float)(Math.Cos(_effectTime) * 2 * MathHelper.Pi);
            _warpEffect.Speed = Constants.Fluctuations.MOVING_WARP_SPEED;
            _warpEffect.Amplitude = _amplitude * Constants.Fluctuations.MOVING_WARP_AMPLITUDE;
        }

        protected override void SoftEnd()
        {
            if (_state == State.In)
            {
                _state = State.Out;
                float inAlpha = _stateTimer.Elapsed / Constants.Fluctuations.MOVING_WARP_TRANSITION_TIME;
                _stateTimer.Reset(Constants.Fluctuations.MOVING_WARP_TRANSITION_TIME);
                _stateTimer.Update((1 - inAlpha) * Constants.Fluctuations.MOVING_WARP_TRANSITION_TIME);
            }
            else if (_state == State.Steady)
            {
                _state = State.Out;
                _stateTimer.Reset(Constants.Fluctuations.MOVING_WARP_TRANSITION_TIME);
            }
        }
    }
}
