using System;
using Microsoft.Xna.Framework;

namespace SuperPong.Processes
{
    public class WaitProcess : Process
    {
        readonly float _duration;
        public float Duration
        {
            get
            {
                return _duration;
            }
        }

        float _time;
        public float Time
        {
            get
            {
                return _time;
            }
        }

        public WaitProcess(float duration)
        {
            _duration = duration;
            _time = duration;
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

        protected override void OnUpdate(GameTime gameTime)
        {
            _time -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_time <= 0)
            {
                Kill();
            }
        }
    }
}
