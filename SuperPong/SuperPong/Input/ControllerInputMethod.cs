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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SuperPong.Common;

namespace SuperPong.Input
{
    public class ControllerInputMethod : InputMethod
    {
        public readonly PlayerIndex PlayerIndex;
        readonly GamePadCapabilities _capabilities;

        public ControllerInputMethod(PlayerIndex playerIndex)
        {
            PlayerIndex = playerIndex;
            _capabilities = GamePad.GetCapabilities(PlayerIndex);
        }

        public override void Update(float dt)
        {
            if (_capabilities.IsConnected)
            {
                GamePadState currentState = GamePad.GetState(PlayerIndex);

                float axis = currentState.ThumbSticks.Left.Y;
                axis = MathUtils.Clamp(-1, 1, axis);

                if (Math.Abs(axis) < Constants.Input.DEADZONE)
                {
                    axis = 0;
                }

                _snapshot._axis = axis;

                // Update join/leave/start
                JoinKeyPressed = currentState.IsButtonDown(Buttons.A);
                LeaveKeyPressed = currentState.IsButtonDown(Buttons.B);
                StartKeyPressed = currentState.IsButtonDown(Buttons.Start);
                PauseKeyPressed = currentState.IsButtonDown(Buttons.Start);
            }
        }
    }
}
