/*
This file is part of Super Pong.

Super Pong is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Super Pong is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Super Pong.  If not, see <http://www.gnu.org/licenses/>.
*/

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
        public static readonly float CAMERA_ROTATE_ZOOM = 0.85f;
        public static readonly float CAMERA_ROTATE_ZOOM_IN_TIME = 1.5f;
        public static readonly float CAMERA_ROTATE_ZOOM_OUT_TIME = CAMERA_ROTATE_ZOOM_IN_TIME;

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
