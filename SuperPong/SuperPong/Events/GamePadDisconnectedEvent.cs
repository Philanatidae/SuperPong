using System;
using Events;
using Microsoft.Xna.Framework;

namespace SuperPong.Events
{
    public class GamePadDisconnectedEvent : IEvent
    {
        public PlayerIndex PlayerIndex
        {
            get;
            private set;
        }

        public GamePadDisconnectedEvent(PlayerIndex playerIndex)
        {
            PlayerIndex = playerIndex;
        }
    }
}
