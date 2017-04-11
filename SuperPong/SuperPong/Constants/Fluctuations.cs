using System;
namespace SuperPong.Constants
{
	public class Fluctuations
	{
		public static readonly int FLUCTUATIONS_COUNT = 1;

		public static readonly float TIMER_MIN = 20;
		public static readonly float TIMER_MAX = 30;

		public static readonly float WARP_IN_TIME = 2.0f;
		public static readonly float WARP_STEADY_TIME = (float)(4 * Math.PI);
		public static readonly float WARP_OUT_TIME = WARP_IN_TIME;
		public static readonly float WARP_SPEED = 60.0f;
        public static readonly float WARP_AMPLITUDE = 0.05f;
	}
}
