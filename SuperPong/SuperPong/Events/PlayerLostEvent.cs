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

        Player LosePlayer;
        Player WinPlayer;
    }
}
