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

namespace SuperPong.Fluctuations
{
    public class CameraWobbleFluctuation : Fluctuation
    {
        enum State
        {
            Wobbling,
            Ending
        }
        State _state = State.Wobbling;

        readonly PongCamera _camera;
        float _elapsedTime;
        float _exitTime;

        public CameraWobbleFluctuation(IPongDirectorOwner owner) : base(owner)
        {
            _camera = _owner.PongCamera;
        }

        protected override void OnKill()
        {
            _camera.ResetRadialDirection();
            _camera.UpdatePositionFromRadial();

            base.OnKill();
        }

        protected override void OnTogglePause()
        {

        }

        protected override void OnUpdate(float dt)
        {
            switch (_state)
            {
                case State.Wobbling:
                    _elapsedTime += dt;

                    _camera.RadialDirection.X = (float)Math.Sin(_elapsedTime
                                                                * Constants.Fluctuations.CAMERA_WOBBLE_SPEED
                                                                * MathHelper.Pi);
                    _camera.RadialDirection.Y = -(float)Math.Sin(_elapsedTime
                                                                * Constants.Fluctuations.CAMERA_WOBBLE_SPEED
                                                                 * MathHelper.TwoPi);
                    _camera.UpdatePositionFromRadial();
                    break;
                case State.Ending:
                    _exitTime += dt;

                    float x = (float)Math.Sin(_elapsedTime
                                                * Constants.Fluctuations.CAMERA_WOBBLE_SPEED
                                                * MathHelper.Pi);
                    float y = -(float)Math.Sin(_elapsedTime
                                                * Constants.Fluctuations.CAMERA_WOBBLE_SPEED
                                               * MathHelper.TwoPi);

                    _camera.RadialDirection.X = x * MathUtils.Clamp(0, 1, 1 - Easings.QuinticEaseInOut(_exitTime / Constants.Fluctuations.CAMERA_WOBBLE_EXIT_TIME));
                    _camera.RadialDirection.Y = y * MathUtils.Clamp(0, 1, 1 - Easings.QuinticEaseInOut(_exitTime / Constants.Fluctuations.CAMERA_WOBBLE_EXIT_TIME));
                    _camera.UpdatePositionFromRadial();
                    break;
            }

            if (_elapsedTime >= Constants.Fluctuations.CAMERA_WOBBLE_STEADY_TIME
                || _exitTime >= Constants.Fluctuations.CAMERA_WOBBLE_EXIT_TIME)
            {
                Kill();
            }
        }

        protected override void SoftEnd()
        {
            _state = State.Ending;
        }
    }
}
