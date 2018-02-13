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

using System.Collections.Generic;
using Events;
using Microsoft.Xna.Framework.Graphics;

namespace SuperPong.UI.Widgets
{
    public class Panel : Widget
    {
        List<Widget> _widgets = new List<Widget>();

        public Panel(Origin origin,
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
        }

        public Panel(Origin origin,
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
        }

        public void Add(Widget widget)
        {
            _widgets.Add(widget);
            widget.Parent = this;
        }

        public void Remove(Widget widget)
        {
            _widgets.Remove(widget);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Hidden)
            {
                for (int i = 0; i < _widgets.Count; i++)
                {
                    _widgets[i].Draw(spriteBatch);
                }
            }
        }

        public override bool Handle(IEvent evt)
        {
            for (int i = 0; i < _widgets.Count; i++)
            {
                if (_widgets[i].Handle(evt))
                {
                    return true;
                }
            }

            return false;
        }

        protected override void OnComputeProperties()
        {
            for (int i = 0; i < _widgets.Count; i++)
            {
                _widgets[i].ComputeProperties();
            }
        }
    }
}
