using System;
using Microsoft.Xna.Framework;

namespace SuperPong.Input.Methods
{
	public abstract class InputMethod
	{
		protected byte _buttons;

		public InputMethod()
		{
		}

		public abstract void Update(GameTime gameTime);

		public InputSnapshot GetSnapshot()
		{
			return new InputSnapshot(_buttons);
		}

	}
}
