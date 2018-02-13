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
    public class Camera : IEventListener
    {
        public float Zoom = 1;
        public Vector2 Position;
        public float Rotation;

        float _compensationZoom = 1;
        Rectangle _bounds;

        public Matrix TransformMatrix
        {
            get
            {
                return Matrix.CreateScale(Zoom * _compensationZoom)
                             * Matrix.CreateRotationZ(Rotation)
                             * Matrix.CreateTranslation(new Vector3(_bounds.Width * 0.5f,
                                                                    _bounds.Height * 0.5f,
                                                                   0))
                             * Matrix.CreateTranslation(new Vector3(Position.X,
                                                            Position.Y * -1,
                                                            0));
            }
        }

        public Camera(float width, float height)
        {
            HandleResize((int)width, (int)height);
        }

        public Vector2 ScreenToWorldCoords(Vector2 position)
        {
            return Vector2.Transform(position, Matrix.Invert(TransformMatrix));
        }

        public Vector2 WorldToScreenCoords(Vector2 position)
        {
            return Vector2.Transform(position, TransformMatrix);
        }

        public bool Handle(IEvent evt)
        {
            ResizeEvent resizeEvt = evt as ResizeEvent;

            if (resizeEvt != null)
            {
                HandleResize(resizeEvt.Width, resizeEvt.Height);
            }

            return false;
        }

        void HandleResize(int w, int h)
        {
            _bounds.Width = w;
            _bounds.Height = h;

            float newAspectRatio = (float)_bounds.Width / _bounds.Height;
            if (newAspectRatio <= Constants.Global.WINDOW_ASPECT_RATIO) // Width is dominant
            {
                _compensationZoom = (float)_bounds.Width / Constants.Global.WINDOW_WIDTH;
            }
            if (newAspectRatio > Constants.Global.WINDOW_ASPECT_RATIO) // Height is dominant
            {
                _compensationZoom = (float)_bounds.Height / Constants.Global.WINDOW_HEIGHT;
            }
        }
    }
}
