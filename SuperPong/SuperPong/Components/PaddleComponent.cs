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

		public float Width = Constants.Pong.PADDLE_WIDTH;
		public float Height = Constants.Pong.PADDLE_HEIGHT;
		public Vector2 Normal;
		public bool IgnoreCollisions;
	}
}
