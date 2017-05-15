using System;
using Events;
using Microsoft.Xna.Framework;
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

        public void UpdatePositionFromRadial()
        {
            float radius = (float)(Constants.Global.SCREEN_ASPECT_RATIO / Math.Tan(MathHelper.ToRadians(FieldOfView) / 2));
            radius *= 0.65f;

            Position = RadialDirection * radius;
        }

        public override bool Handle(IEvent evt)
        {
            ResizeEvent resizeEvt = evt as ResizeEvent;

            if (resizeEvt != null)
            {
                float newAspectRatio = (float)resizeEvt.Width / resizeEvt.Height;
                if (newAspectRatio < Constants.Global.SCREEN_ASPECT_RATIO) // Width is dominant
                {
                    _zoom = (float)resizeEvt.Width / Constants.Global.SCREEN_WIDTH;
                }
                if (newAspectRatio > Constants.Global.SCREEN_ASPECT_RATIO) // Height is dominant
                {
                    _zoom = (float)resizeEvt.Height / Constants.Global.SCREEN_HEIGHT;
                }
            }

            return base.Handle(evt);
        }
    }
}
