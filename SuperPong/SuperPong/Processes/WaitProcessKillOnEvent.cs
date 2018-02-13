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
