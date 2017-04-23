using System;
using SuperPong.Fluctuations;

namespace SuperPong.Constants
{
	public class Fluctuations
	{
		public static readonly float TIMER_MIN = 10;
		public static readonly float TIMER_MAX = 15;

		public static readonly Type[][] FLUCTUATIONS = {
			new Type[] {
				typeof(WarpFluctuation)
			},
			new Type[] {
				typeof(MovingWarpFluctuation)
			}
		};

		public static readonly float WARP_TRANSITION_TIME = 1.0f;
		public static readonly float WARP_STEADY_TIME = (float)(4 * Math.PI);
        public static readonly float WARP_AMPLITUDE = 0.05f;

		public static readonly float MOVING_WARP_TRANSITION_TIME = 1.0f;
		public static readonly float MOVING_WARP_STEADY_TIME = (float)(4 * Math.PI);
		public static readonly float MOVING_WARP_SPEED = 45.0f;
		public static readonly float MOVING_WARP_AMPLITUDE = 0.05f;
	}
}
