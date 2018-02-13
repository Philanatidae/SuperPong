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
using SuperPong.Input;

namespace SuperPong
{
    public class Player
    {
        public int Index
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }

        public InputMethod InputMethod
        {
            get;
            private set;
        }

        public Player(int index, string name) : this(index, name, null)
        {
        }

        public Player(int index, string name, InputMethod inputMethod)
        {
            Index = index;
            Name = name;
            InputMethod = inputMethod;
        }
    }
}
