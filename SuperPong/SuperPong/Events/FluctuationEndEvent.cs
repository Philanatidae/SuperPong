using Events;
using SuperPong.Fluctuations;

namespace SuperPong.Events
{
    public class FluctuationEndEvent : IEvent
    {
        public Fluctuation Fluctuation
        {
            get;
            private set;
        }

        public FluctuationEndEvent(Fluctuation fluctuation)
        {
            Fluctuation = fluctuation;
        }
    }
}
