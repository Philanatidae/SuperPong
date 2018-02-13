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
using Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace SuperPong.UI.Widgets
{
    public class Label : Widget
    {
        readonly BitmapFont _font;
        Vector2 _bounds;
        string _content;
        public string Content
        {
            get
            {
                return _content;
            }
            set
            {
                _content = value;

                Size2 dimensions = _font.MeasureString(_content);
                AspectRatio = dimensions.Width / dimensions.Height;

                _bounds = dimensions;

                ComputeProperties();
            }
        }

        public Label(BitmapFont font,
                      Origin origin,
                      float percentX,
                      float pOffsetX,
                      float percentY,
                      float pOffsetY,
                      float percentAspect,
                      float pOffsetAspect,
                      AspectRatioType aspectRatioType)
            : base(origin, percentX, pOffsetX, percentY, pOffsetY,
                   percentAspect, pOffsetAspect, 0, aspectRatioType)
        {
            _font = font;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Hidden)
            {
                Vector2 scale = new Vector2(Width, Height) / _bounds;
                spriteBatch.DrawString(_font,
                                       _content,
                                       TopLeft,
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
