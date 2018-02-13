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
using SuperPong.Events;

namespace SuperPong.Common
{
    public class PongCamera : PerspectiveCamera
    {
        public new Matrix TransformMatrix
        {
            get
            {
                return Matrix.CreateScale(_zoom) * base.TransformMatrix;
            }
        }

        float _zoom = 1;

        public Vector3 RadialDirection = Vector3.Backward;

        public PongCamera(float width, float height)
            : base(width, height)
        {
            HandleResize((int)width, (int)height);

            ResetRadialDirection();
            UpdatePositionFromRadial();
        }

        public void UpdatePositionFromRadial()
        {
            float radius = (float)(Constants.Global.SCREEN_ASPECT_RATIO / Math.Tan(MathHelper.ToRadians(FieldOfView) / 2));
            radius *= 0.65f;

            Position = RadialDirection * radius;
        }

        public void ResetRadialDirection()
        {
            RadialDirection = Vector3.Backward;
        }

        public override bool Handle(IEvent evt)
        {
            ResizeEvent resizeEvt = evt as ResizeEvent;

            if (resizeEvt != null)
            {
                HandleResize(resizeEvt.Width, resizeEvt.Height);
            }

            return base.Handle(evt);
        }

        void HandleResize(int w, int h)
        {
            /*float newAspectRatio = (float)w / h;
            if (newAspectRatio < Constants.Global.WINDOW_ASPECT_RATIO) // Width is dominant
            {
                _zoom = (float)w / Constants.Global.WINDOW_WIDTH;
            }
            if (newAspectRatio > Constants.Global.WINDOW_ASPECT_RATIO) // Height is dominant
            {
                _zoom = (float)h / Constants.Global.WINDOW_WIDTH;
            }

            _zoom = 1;*/
        }
    }
}
