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

using Microsoft.Xna.Framework;

namespace SuperPong.Processes
{
    public abstract class Process
    {
        bool _kill;
        bool _active;
        bool _paused;
        bool _initialUpdate = true;

        public Process Next
        {
            get;
            private set;
        }

        public bool IsDead()
        {
            return _kill;
        }

        public bool IsActive()
        {
            return _active;
        }
        internal void SetActive(bool active)
        {
            _active = active;
        }

        public bool IsPaused()
        {
            return _paused;
        }

        public bool IsInitialized()
        {
            return !_initialUpdate;
        }

        public Process SetNext(Process process)
        {
            Next = process;
            return Next;
        }

        internal void Update(float dt)
        {
            if (_initialUpdate)
            {
                OnInitialize();
                _initialUpdate = false;
            }
            OnUpdate(dt);
        }
        protected abstract void OnUpdate(float dt);

        protected abstract void OnInitialize();

        public void Kill()
        {
            _kill = true;
            OnKill();
        }
        protected abstract void OnKill();

        public void TogglePause()
        {
            _paused = !_paused;
            OnTogglePause();
        }
        protected abstract void OnTogglePause();
    }
}
