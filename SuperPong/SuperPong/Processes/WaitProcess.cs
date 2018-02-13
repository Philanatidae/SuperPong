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

        protected override void OnUpdate(float dt)
        {
            _time -= dt;

            if (_time <= 0)
            {
                Kill();
            }
        }
    }
}
