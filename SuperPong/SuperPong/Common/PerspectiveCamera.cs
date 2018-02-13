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
    public class PerspectiveCamera : IEventListener
    {
        public Vector3 Position = new Vector3(0, 0, 0);
        public Quaternion Rotation = Quaternion.Identity;

        public Matrix TransformMatrix
        {
            get
            {
                return Matrix.CreateScale(Zoom)
                             * Matrix.CreateFromQuaternion(Rotation)
                             * Matrix.CreateLookAt(Position,
                                           Vector3.Zero,
                                           Vector3.Up);
            }
        }

        public float FieldOfView = 45;
        public float AspectRatio = Constants.Global.SCREEN_ASPECT_RATIO;
        public float Zoom = 1;

        public Matrix PerspectiveMatrix
        {
            get
            {
                return Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(FieldOfView),
                                                           AspectRatio,
                                                           0.1f,
                                                          20);
            }
        }

        public Matrix CombinedMatrix
        {
            get
            {
                return PerspectiveMatrix * TransformMatrix;
            }
        }

        public PerspectiveCamera(float width, float height)
        {
            HandleResize((int)width, (int)height);
        }

        public virtual bool Handle(IEvent evt)
        {
            ResizeEvent resizeEvent = evt as ResizeEvent;
            if (resizeEvent != null)
            {
                HandleResize(resizeEvent.Width, resizeEvent.Height);
            }

            return false;
        }

        void HandleResize(int w, int h)
        {
            AspectRatio = (float)w / h;
        }
    }
}
