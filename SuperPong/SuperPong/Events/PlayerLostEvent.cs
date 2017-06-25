using Events;

namespace SuperPong.Events
{
    public class PlayerLostEvent : IEvent
    {
        public PlayerLostEvent(Player losePlayer, Player winPlayer)
        {
            LosePlayer = losePlayer;
            WinPlayer = winPlayer;
        }

        public Player LosePlayer
        {
            get;
            private set;
        }
        public Player WinPlayer
        {
            get;
            private set;
        }
    }
}
