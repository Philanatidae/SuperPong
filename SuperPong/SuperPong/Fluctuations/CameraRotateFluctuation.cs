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

        public override void SoftEnd()
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
                    _elapsedTime += dt;

                    _camera.RadialDirection.X = (float)Math.Sin(_elapsedTime
                                                                * Constants.Fluctuations.CAMERA_ROTATE_SPEED
                                                                * MathHelper.TwoPi);
                    _camera.RadialDirection.Z = (float)Math.Cos(_elapsedTime
                                                                * Constants.Fluctuations.CAMERA_ROTATE_SPEED
                                                                * MathHelper.TwoPi);
                    _camera.UpdatePositionFromRadial();
                    break;
                case State.Ending:
                    _exitTime += dt;

                    float x = (float)Math.Sin(_elapsedTime
                                                * Constants.Fluctuations.CAMERA_ROTATE_SPEED
                                                * MathHelper.TwoPi);
                    float z = (float)Math.Cos(_elapsedTime
                                                * Constants.Fluctuations.CAMERA_ROTATE_SPEED
                                                * MathHelper.TwoPi);

                    float rad = _elapsedTime * Constants.Fluctuations.CAMERA_ROTATE_SPEED * MathHelper.TwoPi;
                    while (rad > MathHelper.TwoPi)
                    {
                        rad -= MathHelper.TwoPi;
                    }

                    float nrad = rad * MathUtils.Clamp(0, 1, 1 - Easings.QuinticEaseInOut(_exitTime / Constants.Fluctuations.CAMERA_ROTATE_EXIT_TIME));

                    _camera.RadialDirection.X = (float)Math.Sin(nrad);
                    _camera.RadialDirection.Z = (float)Math.Cos(nrad);

                    //_camera.RadialDirection.X = x * MathUtils.Clamp(0, 1, 1 - Easings.QuinticEaseInOut(_exitTime / Constants.Fluctuations.CAMERA_ROTATE_EXIT_TIME));
                    //_camera.RadialDirection.Z = z * MathUtils.Clamp(0, 1, 1 - Easings.QuinticEaseInOut(_exitTime / Constants.Fluctuations.CAMERA_ROTATE_EXIT_TIME));
                    _camera.UpdatePositionFromRadial();
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
