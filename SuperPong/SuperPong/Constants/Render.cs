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
    public class Render
    {
        public static readonly byte GROUP_ONE = 0x1;
        public static readonly byte GROUP_TWO = 0x2;
        public static readonly byte GROUP_THREE = 0x4;
        public static readonly byte GROUP_FOUR = 0x8;
        public static readonly byte GROUP_FIVE = 0x10;
        public static readonly byte GROUP_SIX = 0x20;
        public static readonly byte GROUP_SEVEN = 0x40;
        public static readonly byte GROUP_EIGHT = 0x80;

        public static readonly byte GROUP_MASK_NONE = 0x0;
        public static readonly byte GROUP_MASK_ALL = 0xFF;
    }
}
