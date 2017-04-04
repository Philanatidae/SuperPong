namespace SuperPong.Input
{
	public abstract class InputMethod
	{
		protected InputSnapshot _snapshot = new InputSnapshot();

		public abstract void Update(float dt);

		public InputSnapshot GetSnapshot()
		{
			return _snapshot;
		}

	}
}
