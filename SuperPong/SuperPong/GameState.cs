using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace SuperPong
{
	public abstract class GameState
	{
		protected GameManager _gameManager;
		internal ContentManager Content;

		public GameState(GameManager gameManager)
		{
			_gameManager = gameManager;
		}

		public abstract void LoadContent();

		public void UnloadContent()
		{
			Content.Unload();
		}

		public abstract void Show();

		public abstract void Hide();

		public abstract void Update(GameTime gameTime);

		public abstract void Draw(GameTime gameTime);
	}
}
