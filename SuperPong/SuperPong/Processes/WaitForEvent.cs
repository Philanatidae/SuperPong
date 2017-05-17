using System;
using Events;
using Events.Exceptions;
using Microsoft.Xna.Framework;

namespace SuperPong.Processes
{
    public class WaitForEvent : Process, IEventListener
    {
        readonly Type _eventType;

        public WaitForEvent(Type eventType)
        {
            if (!eventType.IsEvent())
            {
                throw new TypeNotEventException();
            }

            _eventType = eventType;
        }

        public bool Handle(IEvent evt)
        {
            if (evt.GetType().Equals(_eventType))
            {
                Kill();
            }

            return false;
        }

        protected override void OnInitialize()
        {
            EventManager.Instance.RegisterListener(_eventType, this);
        }

        protected override void OnKill()
        {
            EventManager.Instance.UnregisterListener(_eventType, this);
        }

        protected override void OnTogglePause()
        {
        }

        protected override void OnUpdate(float dt)
        {
        }
    }
}
