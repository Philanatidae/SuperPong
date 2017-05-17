﻿using System;
using Events;
using Microsoft.Xna.Framework;
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
                return Matrix.CreateLookAt(Position,
                                           Vector3.Zero,
                                           Vector3.Up)
                             * Matrix.CreateFromQuaternion(Rotation);
            }
        }

        public float FieldOfView = 45;
        public float AspectRatio = Constants.Global.SCREEN_ASPECT_RATIO;

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

        public virtual bool Handle(IEvent evt)
        {
            if (evt is ResizeEvent)
            {
                ResizeEvent resizeEvent = (ResizeEvent)evt;
                AspectRatio = (float)resizeEvent.Width / resizeEvent.Height;
            }

            return false;
        }
    }
}