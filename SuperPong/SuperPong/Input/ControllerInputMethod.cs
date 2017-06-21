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
            }
        }
    }
}
