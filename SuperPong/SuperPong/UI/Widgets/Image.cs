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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperPong.UI.Widgets
{
    public class Image : Widget
    {
        Texture2D _texture;
        Vector2 _bounds;

        public Texture2D Texture
        {
            get
            {
                return _texture;
            }
            set
            {
                _texture = value;
                _bounds = new Vector2(_texture.Width, _texture.Height);
            }
        }

        public Image(Texture2D texture,
                      Origin origin,
                      float percentX,
                      float pOffsetX,
                      float percentY,
                      float pOffsetY,
                      float percentAspect,
                      float pOffsetAspect,
                      float aspectRatio,
                      AspectRatioType aspectRatioType)
            : base(origin, percentX, pOffsetX, percentY, pOffsetY,
                   percentAspect, pOffsetAspect, aspectRatio, aspectRatioType)
        {
            _texture = texture;
            _bounds = new Vector2(_texture.Width, _texture.Height);
        }

        public Image(Texture2D texture,
                      Origin origin,
                      float percentX,
                      float pOffsetX,
                      float percentY,
                      float pOffsetY,
                      float percentWidth,
                      float pOffsetWidth,
                      float percentHeight,
                      float pOffsetHeight)
            : base(origin, percentX, pOffsetX, percentY, pOffsetY,
                 percentWidth, pOffsetWidth, percentHeight, pOffsetHeight)
        {
            _texture = texture;
            _bounds = new Vector2(_texture.Width, _texture.Height);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Hidden)
            {
                Vector2 scale = (BottomRight - TopLeft) / _bounds;
                spriteBatch.Draw(_texture,
                                 TopLeft,
                                 null,
                                 Color.White,
                                 0,
                                 Vector2.Zero,
                                 scale,
                                 SpriteEffects.None,
                                 0);
            }
        }

        public override bool Handle(IEvent evt)
        {
            return false;
        }
    }
}
