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
using MonoGame.Extended.BitmapFonts;

namespace SuperPong.Components
{
    public class FontComponent : IComponent
    {
        public FontComponent()
        {
        }

        public FontComponent(BitmapFont font, string content)
        {
            Font = font;
            Content = content;
        }

        public BitmapFont Font;
        public string Content;
        public Color Color = Color.White;

        public byte RenderGroup = Constants.Render.GROUP_ONE;

        public bool Hidden;
    }
}
