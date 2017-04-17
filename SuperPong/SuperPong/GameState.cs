using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using SuperPong.Content;

namespace SuperPong
{
	public abstract class GameState : IDisposable
	{
		protected GameManager GameManager
		{
			get;
			private set;
		}
		public LockingContentManager Content
		{
			get;
			private set;
		}

		public GameState(GameManager gameManager)
		{
			GameManager = gameManager;

			Content = new LockingContentManager(gameManager.Services);
			Content.RootDirectory = "Content";
		}

		public abstract void Initialize();

		public abstract void LoadContent();

		public void UnloadContent()
		{
			Content.Unload();
		}

		public abstract void Show();

		public abstract void Hide();

		public abstract void Update(GameTime gameTime);

		public abstract void Draw(GameTime gameTime);

		public abstract void Dispose();
	}
}
