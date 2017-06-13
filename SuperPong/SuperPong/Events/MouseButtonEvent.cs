using Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SuperPong.Events
{
    public class MouseButtonEvent : IEvent
    {
        public ButtonState LeftButtonState
        {
            get;
            private set;
        }

        public Vector2 CurrentPosition
        {
            get;
            private set;
        }

        public MouseButtonEvent(ButtonState leftButtonState, Vector2 position)
        {
            LeftButtonState = leftButtonState;
            CurrentPosition = position;
        }
    }
}
