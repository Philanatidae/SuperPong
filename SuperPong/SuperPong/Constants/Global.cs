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
namespace SuperPong.Constants
{
    public class Global
    {
        public static readonly int WINDOW_WIDTH = 960;
        public static readonly int WINDOW_HEIGHT = 600;
        public static readonly float WINDOW_ASPECT_RATIO = (float)WINDOW_WIDTH / WINDOW_HEIGHT;

        public static readonly int SCREEN_WIDTH = 1280;
        public static readonly int SCREEN_HEIGHT = 800;
        public static readonly float SCREEN_ASPECT_RATIO = (float)SCREEN_WIDTH / SCREEN_HEIGHT;

        public static readonly float TICK_RATE = 1 / 120f;
    }
}
