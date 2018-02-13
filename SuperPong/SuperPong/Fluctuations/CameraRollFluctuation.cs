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

using Microsoft.Xna.Framework;
using SuperPong.Common;
using SuperPong.Directors;

namespace SuperPong.Fluctuations
{
    public class CameraRollFluctuation : Fluctuation
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

        public CameraRollFluctuation(IPongDirectorOwner owner) : base(owner)
        {
            _camera = owner.PongCamera;
        }

        protected override void OnKill()
        {
            _camera.Rotation = Quaternion.Identity;

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

                        float alpha = _elapsedTime / Constants.Fluctuations.CAMERA_ROLL_STEADY_TIME;
                        float beta = Easings.SineEaseInOut(alpha);

                        float modElapsedTime = Constants.Fluctuations.CAMERA_ROLL_STEADY_TIME * beta;

                        _rot = modElapsedTime
                              * Constants.Fluctuations.CAMERA_ROLL_SPEED
                            * MathHelper.TwoPi;
                        _camera.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitZ,
                                                                          _rot);

                        // Zoom
                        if (_elapsedTime <= Constants.Fluctuations.CAMERA_ROLL_ZOOM_IN_TIME)
                        {
                            float a = _elapsedTime / Constants.Fluctuations.CAMERA_ROLL_ZOOM_IN_TIME;
                            float b = Easings.SineEaseInOut(a);
                            _zoom = MathHelper.Lerp(1, Constants.Fluctuations.CAMERA_ROLL_ZOOM, b);
                        }
                        else if (_elapsedTime >= Constants.Fluctuations.CAMERA_ROLL_STEADY_TIME
                                - Constants.Fluctuations.CAMERA_ROLL_ZOOM_OUT_TIME)
                        {
                            float a = (Constants.Fluctuations.CAMERA_ROLL_ZOOM_OUT_TIME
                                       - (Constants.Fluctuations.CAMERA_ROLL_STEADY_TIME - _elapsedTime)) / Constants.Fluctuations.CAMERA_ROLL_ZOOM_OUT_TIME;
                            float b = Easings.SineEaseInOut(a);
                            _zoom = MathHelper.Lerp(Constants.Fluctuations.CAMERA_ROLL_ZOOM, 1, b);
                        }
                        else
                        {
                            _zoom = Constants.Fluctuations.CAMERA_ROLL_ZOOM;
                        }
                        _camera.Zoom = _zoom;
                    }
                    break;
                case State.Ending:
                    {
                        _exitTime += dt;
                        float alpha = _exitTime / Constants.Fluctuations.CAMERA_ROLL_EXIT_TIME;

                        // Rotation
                        float currentRot = _rot;
                        currentRot = MathHelper.WrapAngle(currentRot);

                        float targetRot = 0;

                        float rotDiff = MathHelper.WrapAngle(targetRot - currentRot);

                        float rotBeta = Easings.QuinticEaseInOut(alpha);

                        float theta = rotDiff * rotBeta;
                        _camera.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitZ,
                                                                          currentRot + theta);

                        // Zoom
                        float zoomBeta = Easings.QuinticEaseInOut(alpha);
                        float newZoom = MathHelper.Lerp(_zoom, 1, zoomBeta);
                        _camera.Zoom = newZoom;
                    }
                    break;
            }

            if (_elapsedTime >= Constants.Fluctuations.CAMERA_ROLL_STEADY_TIME
               || _exitTime >= Constants.Fluctuations.CAMERA_ROLL_EXIT_TIME)
            {
                Kill();
            }
        }
    }
}
