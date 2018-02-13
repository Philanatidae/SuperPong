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
using MonoGame.Extended.TextureAtlases;
using SuperPong.Events;

namespace SuperPong.UI.Widgets
{
    public class Button : Widget
    {
        public Action Action;

        readonly NinePatchRegion2D _releasedTexture;
        readonly NinePatchRegion2D _hoverTexture;
        readonly NinePatchRegion2D _pressedTexture;
        readonly Vector2 _bounds;

        public Panel SubPanel
        {
            get;
            private set;
        }

        public ButtonState ButtonState
        {
            get;
            private set;
        } = ButtonState.Released;

        public Button(NinePatchRegion2D releasedTexture,
                      NinePatchRegion2D hoverTexture,
                      NinePatchRegion2D pressedTexture,
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
            _releasedTexture = releasedTexture;
            _hoverTexture = hoverTexture;
            _pressedTexture = pressedTexture;
            _bounds = new Vector2(releasedTexture.Width, releasedTexture.Height);

            SubPanel = new Panel(Origin.TopLeft,
                                0,
                                0,
                                0,
                                0,
                                1,
                                0,
                                1,
                                 (float)0)
            {
                Parent = this
            };
        }

        public Button(NinePatchRegion2D releasedTexture,
                      NinePatchRegion2D hoverTexture,
                      NinePatchRegion2D pressedTexture,
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
            _releasedTexture = releasedTexture;
            _hoverTexture = hoverTexture;
            _pressedTexture = pressedTexture;
            _bounds = new Vector2(releasedTexture.Width, releasedTexture.Height);

            SubPanel = new Panel(Origin.TopLeft,
                                0,
                                0,
                                0,
                                0,
                                1,
                                0,
                                1,
                                 (float)0)
            {
                Parent = this
            };
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Hidden)
            {
                NinePatchRegion2D ninePatch = _releasedTexture;
                if (ButtonState == ButtonState.Hover)
                {
                    ninePatch = _hoverTexture;
                }
                if (ButtonState == ButtonState.Pressed)
                {
                    ninePatch = _pressedTexture;
                }

                spriteBatch.Draw(ninePatch,
                                 new Rectangle((int)TopLeft.X,
                                               (int)TopLeft.Y,
                                               (int)Width,
                                               (int)Height),
                                 Color.White);
                SubPanel.Draw(spriteBatch);
            }
        }

        public override bool Handle(IEvent evt)
        {
            MouseMoveEvent mouseMoveEvent = evt as MouseMoveEvent;
            if (mouseMoveEvent != null)
            {
                if (mouseMoveEvent.CurrentPosition.X > TopLeft.X
                    && mouseMoveEvent.CurrentPosition.X < BottomRight.X
                    && mouseMoveEvent.CurrentPosition.Y > TopLeft.Y
                    && mouseMoveEvent.CurrentPosition.Y < BottomRight.Y)
                {
                    if (ButtonState != ButtonState.Pressed)
                    {
                        ButtonState = ButtonState.Hover;
                    }
                }
                else
                {
                    ButtonState = ButtonState.Released;
                }
            }

            MouseButtonEvent mouseButtonEvent = evt as MouseButtonEvent;
            if (mouseButtonEvent != null)
            {
                if (mouseButtonEvent.CurrentPosition.X > TopLeft.X
                    && mouseButtonEvent.CurrentPosition.X < BottomRight.X
                    && mouseButtonEvent.CurrentPosition.Y > TopLeft.Y
                    && mouseButtonEvent.CurrentPosition.Y < BottomRight.Y)
                {
                    if (mouseButtonEvent.LeftButtonState == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    {
                        ButtonState = ButtonState.Pressed;
                    }
                    if (ButtonState == ButtonState.Pressed
                        && mouseButtonEvent.LeftButtonState == Microsoft.Xna.Framework.Input.ButtonState.Released)
                    {
                        if (Action != null && !Hidden)
                        {
                            Action.Invoke();
                        }

                        ButtonState = ButtonState.Released;
                    }
                }
            }

            return false;
        }

        protected override void OnComputeProperties()
        {
            SubPanel.ComputeProperties();
        }
    }

    public enum ButtonState
    {
        Released,
        Hover,
        Pressed
    }
}
