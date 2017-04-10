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

		public float Width = Constants.Pong.BALL_WIDTH;
		public float Height = Constants.Pong.BALL_HEIGHT;

		public float Direction;
		public float Velocity = Constants.Pong.BALL_STARTING_VELOCITY;
	}
}
