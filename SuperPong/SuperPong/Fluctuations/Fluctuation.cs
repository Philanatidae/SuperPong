using Events;
using SuperPong.Directors;
using SuperPong.Events;
using SuperPong.Processes;

namespace SuperPong.Fluctuations
{
    public abstract class Fluctuation : Process, IEventListener
    {
        protected readonly IPongDirectorOwner _owner;

        public enum KillReason
        {
            NORMAL,
            PLAYER_GOAL
        }
        private KillReason _killReason = KillReason.NORMAL;

        public Fluctuation(IPongDirectorOwner _owner)
        {
            this._owner = _owner;
        }

        protected abstract void SoftEnd();

        protected override void OnInitialize()
        {
            EventManager.Instance.QueueEvent(new FluctuationBeginEvent(this));

            EventManager.Instance.RegisterListener<GoalEvent>(this);
        }

        protected override void OnKill()
        {
            EventManager.Instance.QueueEvent(new FluctuationEndEvent(this, _killReason));

            EventManager.Instance.UnregisterListener<GoalEvent>(this);
        }

        public bool Handle(IEvent evt)
        {
            if (evt is GoalEvent)
            {
                _killReason = KillReason.PLAYER_GOAL;
                SoftEnd();
            }

            return false;
        }
    }
}
