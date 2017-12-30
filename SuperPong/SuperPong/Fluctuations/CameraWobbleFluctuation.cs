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
