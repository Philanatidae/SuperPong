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

using Events;
using SuperPong.Directors;
using SuperPong.Events;
using SuperPong.Processes;

namespace SuperPong.Fluctuations
{
    public abstract class Fluctuation : Process, IEventListener
    {
        protected readonly IPongDirectorOwner _owner;

        public enum KillReason
        {
            NORMAL,
            PLAYER_GOAL
        }
        private KillReason _killReason = KillReason.NORMAL;

        public Fluctuation(IPongDirectorOwner _owner)
        {
            this._owner = _owner;
        }

        protected abstract void SoftEnd();

        protected override void OnInitialize()
        {
            EventManager.Instance.QueueEvent(new FluctuationBeginEvent(this));

            EventManager.Instance.RegisterListener<GoalEvent>(this);
        }

        protected override void OnKill()
        {
            EventManager.Instance.QueueEvent(new FluctuationEndEvent(this, _killReason));

            EventManager.Instance.UnregisterListener<GoalEvent>(this);
        }

        public bool Handle(IEvent evt)
        {
            if (evt is GoalEvent)
            {
                _killReason = KillReason.PLAYER_GOAL;
                SoftEnd();
            }

            return false;
        }
    }
}
