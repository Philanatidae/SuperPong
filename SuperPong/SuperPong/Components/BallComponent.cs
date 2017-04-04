using ECS;
using Microsoft.Xna.Framework;

namespace SuperPong.Components
{
	public class BallComponent : IComponent
	{
		public float Width = Constants.Pong.BALL_WIDTH;
		public float Height = Constants.Pong.BALL_HEIGHT;

		public float Direction = MathHelper.ToRadians(Constants.Pong.BALL_STARTING_ROTATION_DEGREES);
		public float Velocity = Constants.Pong.BALL_STARTING_VELOCITY;
	}
}
