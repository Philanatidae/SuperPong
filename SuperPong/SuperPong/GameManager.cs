/*
This file is part of Super Pong.

Super Pong is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Super Pong is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Super Pong.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input.InputListeners;
using SuperPong.Events;
using SuperPong.Input;
using SuperPong.States;

namespace SuperPong
{
    public class GameManager : Game
    {
        GraphicsDeviceManager _graphics;

        InputListenerComponent _inputListenerManager;
        MouseListener _mouseListener;
        GamePadListener _gamePadListener;

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

            Graphics.PreferredBackBufferWidth = (int)Constants.Global.WINDOW_WIDTH;
            Graphics.PreferredBackBufferHeight = (int)Constants.Global.WINDOW_HEIGHT;

            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += Window_ClientSizeChanged;
        }

        protected override void Initialize()
        {
            // Settings
            Settings.Instance.Load();

            Mouse.WindowHandle = Window.Handle;

            IsMouseVisible = true;

            _inputListenerManager = new InputListenerComponent(this);
            Components.Add(_inputListenerManager);

            _mouseListener = new MouseListener();
            _inputListenerManager.Listeners.Add(_mouseListener);

            _mouseListener.MouseMoved += Mouse_MouseMoved;
            _mouseListener.MouseDown += Mouse_MouseDownOrUp;
            _mouseListener.MouseUp += Mouse_MouseDownOrUp;

            _gamePadListener = new GamePadListener();
            _inputListenerManager.Listeners.Add(_gamePadListener);

            GamePadListener.CheckControllerConnections = true;
            GamePadListener.ControllerConnectionChanged += GamePad_ControllerConnectionChanged;

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

        void GamePad_ControllerConnectionChanged(object sender, GamePadEventArgs e)
        {
            // More than 4 controllers not supported
            if ((int)e.PlayerIndex < 4)
            {
                if (!e.PreviousState.IsConnected
                   && e.CurrentState.IsConnected)
                {
                    // Controller connected
                    EventManager.Instance.QueueEvent(new GamePadConnectedEvent(e.PlayerIndex));
                }
                if (e.PreviousState.IsConnected
                   && !e.CurrentState.IsConnected)
                {
                    // Controller disconnected
                    EventManager.Instance.QueueEvent(new GamePadDisconnectedEvent(e.PlayerIndex));
                }
            }
        }
    }
}
