using Microsoft.Xna.Framework;

namespace SuperPong.Processes
{
    public abstract class IntervalProcess : Process
    {
        float _acculmulator;
        readonly float _interval;

        public float Interval
        {
            get
            {
                return _interval;
            }
        }

        public IntervalProcess(float interval)
        {
            _interval = interval;
        }

        protected override void OnInitialize()
        {
        }

        protected override void OnKill()
        {
        }

        protected override void OnTogglePause()
        {
        }

        protected override void OnUpdate(float dt)
        {
            _acculmulator += dt;
            while (_acculmulator >= _interval)
            {
                OnTick(_interval);
                _acculmulator -= _interval;
            }
        }

        protected abstract void OnTick(float interval);
    }
}
