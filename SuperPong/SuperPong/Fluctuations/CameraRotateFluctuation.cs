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
    public class CameraRotateFluctuation : Fluctuation
    {
        enum State
        {
            Rotating,
            Ending
        }
        State _state = State.Rotating;

        readonly PongCamera _camera;
        float _elapsedTime;
        float _exitTime;

        float _rot;
        float _zoom = 1;

        public CameraRotateFluctuation(IPongDirectorOwner owner) : base(owner)
        {
            _camera = _owner.PongCamera;
        }

        protected override void OnKill()
        {
            _camera.ResetRadialDirection();
            _camera.UpdatePositionFromRadial();

            base.OnKill();
        }

        protected override void SoftEnd()
        {
            _state = State.Ending;
        }

        protected override void OnTogglePause()
        {
        }

        protected override void OnUpdate(float dt)
        {
            switch (_state)
            {
                case State.Rotating:
                    {
                        _elapsedTime += dt;

                        float alpha = _elapsedTime / Constants.Fluctuations.CAMERA_ROTATE_STEADY_TIME;
                        float beta = Easings.SineEaseInOut(alpha);

                        float modElapsedTime = Constants.Fluctuations.CAMERA_ROTATE_STEADY_TIME * beta;

                        _rot = modElapsedTime
                                           * Constants.Fluctuations.CAMERA_ROTATE_SPEED
                                           * MathHelper.TwoPi;

                        _camera.RadialDirection.X = (float)Math.Sin(_rot);
                        _camera.RadialDirection.Z = (float)Math.Cos(_rot);
                        _camera.UpdatePositionFromRadial();

                        // Zoom
                        if (_elapsedTime <= Constants.Fluctuations.CAMERA_ROTATE_ZOOM_IN_TIME)
                        {
                            float a = _elapsedTime / Constants.Fluctuations.CAMERA_ROTATE_ZOOM_IN_TIME;
                            float b = Easings.SineEaseInOut(a);
                            _zoom = MathHelper.Lerp(1, Constants.Fluctuations.CAMERA_ROTATE_ZOOM, b);
                        }
                        else if (_elapsedTime >= Constants.Fluctuations.CAMERA_ROTATE_STEADY_TIME
                                - Constants.Fluctuations.CAMERA_ROTATE_ZOOM_OUT_TIME)
                        {
                            float a = (Constants.Fluctuations.CAMERA_ROTATE_ZOOM_OUT_TIME
                                       - (Constants.Fluctuations.CAMERA_ROTATE_STEADY_TIME - _elapsedTime)) / Constants.Fluctuations.CAMERA_ROLL_ZOOM_OUT_TIME;
                            float b = Easings.SineEaseInOut(a);
                            _zoom = MathHelper.Lerp(Constants.Fluctuations.CAMERA_ROTATE_ZOOM, 1, b);
                        }
                        else
                        {
                            _zoom = Constants.Fluctuations.CAMERA_ROTATE_ZOOM;
                        }
                        _camera.Zoom = _zoom;
                    }
                    break;
                case State.Ending:
                    {
                        _exitTime += dt;

                        float currRot = _rot;
                        currRot = MathHelper.WrapAngle(currRot);

                        float targetRot = 0;

                        float rotDiff = MathHelper.WrapAngle(targetRot - currRot);

                        float alpha = _exitTime / Constants.Fluctuations.CAMERA_ROTATE_EXIT_TIME;
                        float beta = Easings.QuinticEaseInOut(alpha);

                        float nrot = currRot + rotDiff * beta;
                        _camera.RadialDirection.X = (float)Math.Sin(nrot);
                        _camera.RadialDirection.Z = (float)Math.Cos(nrot);
                        _camera.UpdatePositionFromRadial();

                        // Zoom
                        float zoomBeta = Easings.QuinticEaseInOut(alpha);
                        float newZoom = MathHelper.Lerp(_zoom, 1, zoomBeta);
                        _camera.Zoom = newZoom;
                    }
                    break;
            }

            if (_elapsedTime >= Constants.Fluctuations.CAMERA_ROTATE_STEADY_TIME
               || _exitTime >= Constants.Fluctuations.CAMERA_ROTATE_EXIT_TIME)
            {
                Kill();
            }
        }
    }
}
