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
