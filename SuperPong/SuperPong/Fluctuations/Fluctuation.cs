using Events;
using SuperPong.Directors;
using SuperPong.Events;
using SuperPong.Processes;

namespace SuperPong.Fluctuations
{
	public abstract class Fluctuation : Process
	{
		protected readonly IPongDirectorOwner _owner;

		public Fluctuation(IPongDirectorOwner _owner)
		{
			this._owner = _owner;
		}

		protected override void OnInitialize()
		{
			EventManager.Instance.QueueEvent(new FluctuationBeginEvent(this));
		}

		protected override void OnKill()
		{
			EventManager.Instance.QueueEvent(new FluctuationEndEvent(this));
		}
	}
}
