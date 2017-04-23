using Microsoft.Xna.Framework;

namespace SuperPong.Constants
{
	public class Pong
	{
		public static readonly byte RENDER_GROUP = Render.GROUP_TWO;

		public static readonly Vector2 BUFFER_RENDER_POSITION = new Vector2(0, -25);

		public static readonly float PLAYFIELD_HEIGHT = 450;
		public static readonly float PLAYFIELD_WIDTH = PLAYFIELD_HEIGHT * 1.5f;

		public static readonly float GOAL_WIDTH = 3f;
		public static readonly float GOAL_HEIGHT = PLAYFIELD_HEIGHT;

		public static readonly float EDGE_WIDTH = PLAYFIELD_WIDTH;
		public static readonly float EDGE_HEIGHT = 3f;

		public static readonly int LIVES_LEFT_POSITION_X = -250;
		public static readonly int LIVES_RIGHT_POSITION_X = 215;
		public static readonly int LIVES_ICON_LEFT_POSITION_X = LIVES_LEFT_POSITION_X + 35;
		public static readonly int LIVES_ICON_RIGHT_POSITION_X = LIVES_RIGHT_POSITION_X + 35;
		public static readonly int LIVES_POSITION_Y = 265;
		public static readonly int LIVES_COUNT = 5;

		public static readonly float PADDLE_WIDTH = 10;
		public static readonly float PADDLE_HEIGHT = 60;
		public static readonly float PADDLE_SPEED = 350;
		public static readonly float PADDLE_STARTING_X = PLAYFIELD_WIDTH / 2 - 2.5f * PADDLE_WIDTH;
		public static readonly float PADDLE_STARTING_Y = 0;

		public static readonly float BALL_WIDTH = 15;
		public static readonly float BALL_HEIGHT = 15;
		public static readonly float BALL_PLAYER1_STARTING_ROTATION_DEGREES = 135;
		public static readonly float BALL_PLAYER2_STARTING_ROTATION_DEGREES = 45;
		public static readonly float BALL_STARTING_VELOCITY = 350.0f;
		public static readonly float BALL_MAX_TRAVEL_ANGLE_DEGREES = 60.0f;
		public static readonly float BALL_SPEED_INCREASE = 10.0f;

	}
}
