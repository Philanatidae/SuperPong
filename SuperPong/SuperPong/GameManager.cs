
using Microsoft.Xna.Framework;

namespace SuperPong
{
	public class GameManager : Game
	{
		private GraphicsDeviceManager _graphics;

		private GameState _currentState;

		public GraphicsDeviceManager Graphics
		{
			get
			{
				return _graphics;
			}
		}

		public GameManager()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		protected override void Initialize()
		{
			base.Initialize();
		}

		protected override void LoadContent()
		{
			// Global Content
		}

		protected override void Update(GameTime gameTime)
		{
			if (_currentState != null)
			{
				_currentState.Update(gameTime);
			}

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			_graphics.GraphicsDevice.Clear(Color.Black);

			if (_currentState != null)
			{
				_currentState.Draw(gameTime);
			}

			base.Draw(gameTime);
		}

		public void changeState(GameState nextState)
		{
			if (_currentState != null)
			{
				_currentState.Hide();
				_currentState.UnloadContent();
			}

			_currentState = nextState;

			_currentState.Content = new Microsoft.Xna.Framework.Content.ContentManager(Services);
			_currentState.LoadContent();
			_currentState.Show();
		}
	}
}
