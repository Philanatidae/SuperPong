using SuperPong.Directors;
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
	}
}
