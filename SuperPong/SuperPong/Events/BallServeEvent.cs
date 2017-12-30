using System;
using ECS;
using Events;
using Microsoft.Xna.Framework;

namespace SuperPong.Events
{
    public class BallServeEvent : IEvent
    {
        public Entity Ball
        {
            get;
            internal set;
        }

        public Vector2 Position
        {
            get;
            internal set;
        }

        public BallServeEvent(Entity ball, Vector2 position)
        {
            Ball = ball;
            Position = position;
        }
    }
}
