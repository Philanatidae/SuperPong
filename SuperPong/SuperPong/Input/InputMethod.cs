using System;
using Microsoft.Xna.Framework;

namespace SuperPong.Input
{
	public abstract class InputMethod
	{
		protected InputSnapshot _snapshot;

		public abstract void Update(GameTime gameTime);

		public InputSnapshot GetSnapshot()
		{
			return _snapshot;
		}

	}
}
