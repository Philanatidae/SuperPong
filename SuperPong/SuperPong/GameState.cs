using System;

using Microsoft.Xna.Framework.Content;

namespace SuperPong
{
	public abstract class GameState
	{
		protected ContentManager content;

		public GameState()
		{
		}

		public abstract void LoadContent();

		public abstract void UnloadContent();
	}
}
