using Events;
using Microsoft.Xna.Framework;

namespace SuperPong.Events
{
    public class GamePadConnectedEvent : IEvent
    {
        public PlayerIndex PlayerIndex
        {
            get;
            private set;
        }

        public GamePadConnectedEvent(PlayerIndex playerIndex)
        {
            PlayerIndex = playerIndex;
        }
    }
}
