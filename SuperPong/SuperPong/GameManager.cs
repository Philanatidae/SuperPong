using System;
using Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input.InputListeners;
using SuperPong.Events;
using SuperPong.States;

namespace SuperPong
{
    public class GameManager : Game
    {
        GraphicsDeviceManager _graphics;

        InputListenerComponent _inputListenerManager;
        MouseListener _mouseListener;

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
            Graphics.PreferMultiSampling = true;

            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += Window_ClientSizeChanged;
        }

        protected override void Initialize()
        {
            Mouse.WindowHandle = Window.Handle;

            IsMouseVisible = true;

            _inputListenerManager = new InputListenerComponent(this);
            Components.Add(_inputListenerManager);

            _mouseListener = new MouseListener();
            _inputListenerManager.Listeners.Add(_mouseListener);

            _mouseListener.MouseMoved += Mouse_MouseMoved;
            _mouseListener.MouseDown += Mouse_MouseDownOrUp;
            _mouseListener.MouseUp += Mouse_MouseDownOrUp;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Global Content

            // Load first game state
            ChangeState(new MenuGameState(this));
        }

        protected override void Update(GameTime gameTime)
        {
            EventManager.Instance.Dispatch();

            if (_currentState != null)
            {
                _currentState.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _graphics.GraphicsDevice.Clear(Color.Black);

            GraphicsDevice.RasterizerState = RasterizerState.CullNone;

            if (_currentState != null)
            {
                _currentState.Draw((float)gameTime.ElapsedGameTime.TotalSeconds);
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

        void Mouse_MouseMoved(object sender, MouseEventArgs e)
        {
            EventManager.Instance.QueueEvent(new MouseMoveEvent(new Vector2(e.PreviousState.Position.X,
                                                                           e.PreviousState.Position.Y),
                                                                new Vector2(e.Position.X,
                                                                           e.Position.Y)));
        }
        void Mouse_MouseDownOrUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButton.Left)
            {
                EventManager.Instance.QueueEvent(new MouseButtonEvent(e.CurrentState.LeftButton,
                                                                      new Vector2(e.Position.X,
                                                                                  e.Position.Y)));
            }
        }
    }
}
