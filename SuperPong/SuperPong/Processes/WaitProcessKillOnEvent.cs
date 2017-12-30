using System;
using Events;

namespace SuperPong.Processes
{
    public class WaitProcessKillOnEvent : WaitProcess, IEventListener
    {
        Type _killEvent;

        public WaitProcessKillOnEvent(float duration, Type killEvent) : base(duration)
        {
            _killEvent = killEvent;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            EventManager.Instance.RegisterListener(_killEvent, this);
        }

        protected override void OnKill()
        {
            base.OnKill();

            EventManager.Instance.UnregisterListener(_killEvent, this);
        }

        public bool Handle(IEvent evt)
        {
            if (evt.GetType().IsAssignableFrom(_killEvent))
            {
                Kill();

                // Note that if killed this way, all sequenced processes
                // are killed as well.
                SetNext(null);
            }

            return false;
        }
    }
}
