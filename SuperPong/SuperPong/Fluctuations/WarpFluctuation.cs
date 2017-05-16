using Microsoft.Xna.Framework;
using SuperPong.Common;
using SuperPong.Directors;
using SuperPong.Graphics.PostProcessor.Effects;

namespace SuperPong.Fluctuations
{
    public class WarpFluctuation : Fluctuation
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

        public WarpFluctuation(IPongDirectorOwner owner) : base(owner)
        {
            _warpEffect = new VerticalWarp(_owner.PongPostProcessor, _owner.Content);
        }

        protected override void OnInitialize()
        {
            _stateTimer = new Timer(Constants.Fluctuations.WARP_TRANSITION_TIME);

            _warpEffect.Time = 0;
            _warpEffect.Speed = 0;

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

        protected override void OnUpdate(GameTime gameTime)
        {
            _stateTimer.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            switch (_state)
            {
                case State.In:
                    _amplitude = MathUtils.Clamp(0, 1, Easings.QuarticEaseOut(_stateTimer.Elapsed / Constants.Fluctuations.WARP_TRANSITION_TIME));

                    if (_stateTimer.HasElapsed())
                    {
                        _state = State.Steady;
                        _stateTimer.Reset(Constants.Fluctuations.WARP_STEADY_TIME);
                    }
                    break;
                case State.Steady:
                    _effectTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (_stateTimer.HasElapsed())
                    {
                        _state = State.Out;
                        _stateTimer.Reset(Constants.Fluctuations.WARP_TRANSITION_TIME);
                    }
                    break;
                case State.Out:
                    _amplitude = MathUtils.Clamp(0, 1, 1 - Easings.QuarticEaseOut(_stateTimer.Elapsed / Constants.Fluctuations.WARP_TRANSITION_TIME));

                    if (_stateTimer.HasElapsed())
                    {
                        Kill();
                    }
                    break;
            }

            _warpEffect.Amplitude = _amplitude * Constants.Fluctuations.WARP_AMPLITUDE;
        }

        public override void SoftEnd()
        {
            if (_state == State.In)
            {
                _state = State.Out;
                float inAlpha = _stateTimer.Elapsed / Constants.Fluctuations.WARP_TRANSITION_TIME;
                _stateTimer.Reset(Constants.Fluctuations.WARP_TRANSITION_TIME);
                _stateTimer.Update((1 - inAlpha) * Constants.Fluctuations.WARP_TRANSITION_TIME);
            }
            else if (_state == State.Steady)
            {
                _state = State.Out;
                _stateTimer.Reset(Constants.Fluctuations.WARP_TRANSITION_TIME);
            }
        }
    }
}
