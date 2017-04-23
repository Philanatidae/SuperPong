using ECS;
using Events;
using Microsoft.Xna.Framework;

namespace SuperPong.Events
{
    public class BallBounceEvent : IEvent
    {
        public Entity Ball
        {
            get;
            internal set;
        }

        public Entity Collider
        {
            get;
            internal set;
        }

        public Vector2 Position
        {
            get;
            internal set;
        }

        public BallBounceEvent(Entity ball, Entity collider, Vector2 position)
        {
            Ball = ball;
            Collider = collider;
            Position = position;
        }
    }
}
