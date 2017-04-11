namespace SuperPong
{
	public class Timer
	{
		float _duration;
		float _elapsedTime;
		public bool Enabled
		{
			get;
			set;
		} = true;

		public float Elapsed
		{
			get
			{
				return _elapsedTime;
			}
		}

		public Timer(float duration)
		{
			_duration = duration;
		}

		public void Update(float dt)
		{
			if (Enabled)
			{
				_elapsedTime += dt;
			}
		}

		public bool HasElapsed()
		{
			return _elapsedTime >= _duration && Enabled;
		}

		public void Reset()
		{
			_elapsedTime = 0;
		}

		public void Reset(float newDuration)
		{
			_duration = newDuration;
			Reset();
		}
	}
}
