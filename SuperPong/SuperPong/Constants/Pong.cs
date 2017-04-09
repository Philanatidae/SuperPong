using Microsoft.Xna.Framework;

namespace SuperPong.Constants
{
	public class Pong
	{
		public static readonly byte RENDER_GROUP = Render.GROUP_TWO;

		public static readonly Vector2 BUFFER_RENDER_POSITION = new Vector2(0, -65);

		public static readonly float PLAYFIELD_WIDTH = 600;
		public static readonly float PLAYFIELD_HEIGHT = 400;

		public static readonly float GOAL_WIDTH = 3f;

		public static readonly float EDGE_HEIGHT = 3f;

		public static readonly float PADDLE_WIDTH = 10;
		public static readonly float PADDLE_HEIGHT = 60;
		public static readonly float PADDLE_SPEED = 240;
		public static readonly float PADDLE_STARTING_X = PLAYFIELD_WIDTH / 2 - 2.5f * PADDLE_WIDTH;
		public static readonly float PADDLE_STARTING_Y = 0;

		public static readonly float BALL_WIDTH = 15;
		public static readonly float BALL_HEIGHT = 15;
		public static readonly float BALL_STARTING_ROTATION_DEGREES = 45;
		public static readonly float BALL_STARTING_VELOCITY = 200.0f;
		public static readonly float BALL_MAX_TRAVEL_ANGLE_DEGREES = 60.0f;

	}
}
