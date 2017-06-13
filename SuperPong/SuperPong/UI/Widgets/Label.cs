﻿using System;
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
                      float percentHeight)
            : base(origin, percentX, pOffsetX, percentY, pOffsetY,
                   percentHeight, 0, 0, AspectRatioType.HeightMaster)
        {
            _font = font;
        }

        public override void Draw(SpriteBatch spriteBatch)
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

        public override bool Handle(IEvent evt)
        {
            return false;
        }
    }
}
