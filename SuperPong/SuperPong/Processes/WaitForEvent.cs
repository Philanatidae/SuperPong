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
