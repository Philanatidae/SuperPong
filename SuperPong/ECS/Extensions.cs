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
namespace ECS
{
    public static class Extensions
    {
        public static bool IsComponent(this Type type)
        {
            return typeof(IComponent).IsAssignableFrom(type);
        }

        public static bool IsEquivalent(this object[] a, object[] b)
        {
            if (a.Length != b.Length)
            {
                return false;
            }

            foreach (object objA in a)
            {
                bool found = false;
                foreach (object objB in b)
                {
                    if (objA.Equals(objB))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
