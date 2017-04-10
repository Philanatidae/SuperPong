using Microsoft.Xna.Framework;

namespace SuperPong.Processes
{
	public abstract class Process
	{
		bool _kill;
		bool _active;
		bool _paused;
		bool _initialUpdate = true;

		public Process Next
		{
			get;
			private set;
		}

		public bool IsDead()
		{
			return _kill;
		}

		public bool IsActive()
		{
			return _active;
		}
		internal void SetActive(bool active)
		{
			_active = active;
		}

		public bool IsPaused()
		{
			return _paused;
		}

		public bool IsInitialized()
		{
			return !_initialUpdate;
		}

		public Process SetNext(Process process)
		{
			Next = process;
			return Next;
		}

		internal void Update(GameTime gameTime)
		{
			if (_initialUpdate)
			{
				OnInitialize();
				_initialUpdate = true;
			}
			OnUpdate(gameTime);
		}
		protected abstract void OnUpdate(GameTime gameTime);

		protected abstract void OnInitialize();

		public void Kill()
		{
			_kill = true;
			OnKill();
		}
		protected abstract void OnKill();

		public void TogglePause()
		{
			_paused = !_paused;
			OnTogglePause();
		}
		protected abstract void OnTogglePause();
	}
}
