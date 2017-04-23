using ECS;
using Events;
using Microsoft.Xna.Framework;

namespace SuperPong.Events
{
    public class GoalEvent : IEvent
    {
        public Entity Ball
        {
            get;
            private set;
        }

        public Entity Goal
        {
            get;
            private set;
        }

        public Vector2 Position
        {
            get;
            private set;
        }

        public GoalEvent(Entity ball, Entity goal, Vector2 position)
        {
            Ball = ball;
            Goal = goal;
            Position = position;
        }
    }
}
