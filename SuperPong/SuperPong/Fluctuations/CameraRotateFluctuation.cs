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

                    float currRot = _elapsedTime * Constants.Fluctuations.CAMERA_ROTATE_SPEED * MathHelper.TwoPi;
                    currRot = MathHelper.WrapAngle(currRot);

                    float targetRot = 0;

                    float rotDiff = MathHelper.WrapAngle(targetRot - currRot);

                    float alpha = _exitTime / Constants.Fluctuations.CAMERA_ROTATE_EXIT_TIME;
                    float beta = Easings.QuinticEaseInOut(alpha);

                    float nrot = currRot + rotDiff * beta;
                    _camera.RadialDirection.X = (float)Math.Sin(nrot);
                    _camera.RadialDirection.Z = (float)Math.Cos(nrot);
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
