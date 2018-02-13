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

using ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperPong.Components
{
    public class SpriteComponent : IComponent
    {
        public SpriteComponent()
        {
        }

        public SpriteComponent(Texture2D texture, Vector2 bounds)
        {
            Texture = texture;
            Bounds = bounds;
        }

        public Texture2D Texture;
        public Vector2 Bounds;

        public byte RenderGroup = 0x1;

        public bool Hidden;
    }
}
