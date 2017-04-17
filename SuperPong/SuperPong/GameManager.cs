using System;
using Events;
using Microsoft.Xna.Framework;
using SuperPong.Events;
using SuperPong.Input;

namespace SuperPong
{
	public class GameManager : Game
	{
		GraphicsDeviceManager _graphics;

		GameState _currentState;

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

			Graphics.PreferredBackBufferWidth = (int)Constants.Global.SCREEN_WIDTH;
			Graphics.PreferredBackBufferHeight = (int)Constants.Global.SCREEN_HEIGHT;

			Window.AllowUserResizing = true;
			Window.ClientSizeChanged += Window_ClientSizeChanged;
		}

		protected override void Initialize()
		{
			IsMouseVisible = true;

			base.Initialize();
		}

		protected override void LoadContent()
		{
			// Global Content

			// Load first game state
			Player player1 = new Player(0, "Player 1", new PrimaryKeyboardInputMethod());
			Player player2 = new Player(1, "Player 2", new SecondaryKeyboardInputMethod());
			ChangeState(new MainGameState(this, player1, player2));
		}

		protected override void Update(GameTime gameTime)
		{
			if (_currentState != null)
			{
				_currentState.Update(gameTime);
			}

			EventManager.Instance.Dispatch();

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

		public void ChangeState(GameState nextState)
		{
			if (_currentState != null)
			{
				_currentState.Hide();
				_currentState.UnloadContent();
				_currentState.Dispose();
			}

			_currentState = nextState;
			_currentState.Initialize();

			_currentState.LoadContent();
			_currentState.Content.Locked = true;
			_currentState.Show();
		}

		void Window_ClientSizeChanged(object sender, EventArgs e)
		{
			EventManager.Instance.QueueEvent(new ResizeEvent(Window.ClientBounds.Width,
			                                                   Window.ClientBounds.Height));
		}
	}
}
