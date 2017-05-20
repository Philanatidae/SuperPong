using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SuperPong.Common;

namespace SuperPong.Input
{
    public class ControllerInputMethod : InputMethod
    {
        readonly PlayerIndex _playerIndex;
        readonly GamePadCapabilities _capabilities;

        public ControllerInputMethod(PlayerIndex playerIndex)
        {
            _playerIndex = playerIndex;
            _capabilities = GamePad.GetCapabilities(_playerIndex);
        }

        public override void Update(float dt)
        {
            if (_capabilities.IsConnected)
            {
                GamePadState currentState = GamePad.GetState(_playerIndex);

                float axis = currentState.ThumbSticks.Left.Y;
                axis = MathUtils.Clamp(-1, 1, axis);

                if (Math.Abs(axis) < Constants.Input.DEADZONE)
                {
                    axis = 0;
                }

                _snapshot._axis = axis;
            }
        }
    }
}
