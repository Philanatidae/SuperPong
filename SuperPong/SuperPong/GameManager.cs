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

			Graphics.PreferredBackBufferWidth = 960;
			Graphics.PreferredBackBufferHeight = 600;

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
			ChangeState(new MainGameState(this, new PrimaryKeyboardInputMethod(), new SecondaryKeyboardInputMethod()));
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

		public void ChangeState(GameState nextState)
		{
			if (_currentState != null)
			{
				_currentState.Hide();
				_currentState.UnloadContent();
			}

			_currentState = nextState;
			_currentState.Initialize();

			_currentState.LoadContent();
			_currentState.Show();
		}

		void Window_ClientSizeChanged(object sender, EventArgs e)
		{
			EventManager.Instance.TriggerEvent(new ResizeEvent(Window.ClientBounds.Width,
			                                                   Window.ClientBounds.Height));
		}
	}
}
