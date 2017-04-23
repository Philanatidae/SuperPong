using ECS;
using Microsoft.Xna.Framework;

namespace SuperPong.Components
{
    public class PaddleComponent : IComponent
    {
        public PaddleComponent()
        {
        }

        public PaddleComponent(Vector2 normal)
        {
            Normal = normal;
        }

        public Vector2 Bounds = new Vector2(Constants.Pong.PADDLE_WIDTH, Constants.Pong.PADDLE_HEIGHT);
        public Vector2 Normal;
        public bool IgnoreCollisions;
    }
}
