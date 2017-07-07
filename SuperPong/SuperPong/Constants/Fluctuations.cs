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
                typeof(CameraWobbleFluctuation),
                typeof(WarpFluctuation),
                typeof(CameraRollFluctuation)
            },
            new Type[] {
                typeof(MovingWarpFluctuation),
                typeof(CameraRotateFluctuation)
            }
        };

        public static readonly float CAMERA_WOBBLE_SPEED = 0.1f;
        public static readonly float CAMERA_WOBBLE_STEADY_TIME = 2 / CAMERA_WOBBLE_SPEED;
        public static readonly float CAMERA_WOBBLE_EXIT_TIME = 0.5f;

        public static readonly float CAMERA_ROTATE_SPEED = 0.1f;
        public static readonly float CAMERA_ROTATE_STEADY_TIME = 2 / CAMERA_ROTATE_SPEED;
        public static readonly float CAMERA_ROTATE_EXIT_TIME = 0.5f;

        public static readonly float CAMERA_ROLL_SPEED = 0.1f;
        public static readonly float CAMERA_ROLL_STEADY_TIME = 2 / CAMERA_ROLL_SPEED;
        public static readonly float CAMERA_ROLL_EXIT_TIME = 1;
        public static readonly float CAMERA_ROLL_ZOOM = 0.7f;
        public static readonly float CAMERA_ROLL_ZOOM_IN_TIME = 1.5f;
        public static readonly float CAMERA_ROLL_ZOOM_OUT_TIME = CAMERA_ROLL_ZOOM_IN_TIME;

        public static readonly float WARP_TRANSITION_TIME = 1.0f;
        public static readonly float WARP_STEADY_TIME = (float)(4 * Math.PI);
        public static readonly float WARP_AMPLITUDE = 0.05f;

        public static readonly float MOVING_WARP_TRANSITION_TIME = 1.0f;
        public static readonly float MOVING_WARP_STEADY_TIME = (float)(4 * Math.PI);
        public static readonly float MOVING_WARP_SPEED = 45.0f;
        public static readonly float MOVING_WARP_AMPLITUDE = 0.05f;
    }
}
