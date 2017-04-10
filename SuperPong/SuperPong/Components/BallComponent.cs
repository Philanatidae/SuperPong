using ECS;
using Microsoft.Xna.Framework;

namespace SuperPong.Components
{
	public class BallComponent : IComponent
	{
		public BallComponent(float direction)
		{
			Direction = MathHelper.ToRadians(direction);
		}

		public Vector2 Bounds = new Vector2(Constants.Pong.BALL_WIDTH, Constants.Pong.BALL_HEIGHT);

		public float Direction;
		public float Velocity = Constants.Pong.BALL_STARTING_VELOCITY;
	}
}
