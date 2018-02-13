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

using ECS;
using Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using SuperPong.Common;
using SuperPong.Components;
using SuperPong.Directors;
using SuperPong.Entities;
using SuperPong.Events;
using SuperPong.Graphics;
using SuperPong.Graphics.PostProcessor;
using SuperPong.Systems;
using SuperPong.Particles;
using SuperPong.Processes;
using SuperPong.Processes.Animations;
using System;
using SuperPong.States;
using SuperPong.UI;
using SuperPong.UI.Widgets;

namespace SuperPong
{
    public class MainGameState : GameState, IPongDirectorOwner, IEventListener
    {
        Player _player1;
        Player _player2;

        ProcessManager _processManager;

        float _acculmulator;

        Engine _engine;
        AIThinkSystem _aiSystem;
        InputSystem _inputSystem;
        PaddleSystem _paddleSystem;
        BallMovementSystem _ballMovementSystem;
        GoalSystem _goalSystem;
        LivesSystem _livesSystem;
        RenderSystem _renderSystem;

        BaseDirector[] _directors;

        Camera _mainCamera;
        PongCamera _pongCamera;

        RenderTarget2D _pongRenderTarget;

        BasicEffect _quadEffect;
        Quad _quad;

        public Engine Engine
        {
            get
            {
                return _engine;
            }
        }

        public Player Player1
        {
            get
            {
                return _player1;
            }
        }

        public Player Player2
        {
            get
            {
                return _player2;
            }
        }

        public PostProcessor PongPostProcessor
        {
            get;
            private set;
        }

        public ParticleManager<VelocityParticleInfo> VelocityParticleManager
        {
            get;
            private set;
        }

        public PongCamera PongCamera
        {
            get
            {
                return _pongCamera;
            }
        }

        ContentManager IPongDirectorOwner.Content
        {
            get
            {
                return Content;
            }
        }

        bool _paused;
        public bool Paused
        {
            get
            {
                return _paused;
            }
            set
            {
                _paused = value;
                _pausedBackground.Hidden = !Paused;
                _pausedLabel.Hidden = !Paused;
            }
        }
        bool _pauseKeyReleased;

        Root _root;
        Image _pausedBackground;
        Label _pausedLabel;

        public MainGameState(GameManager gameManager,
                             Player player1,
                             Player player2)
            : base(gameManager)
        {
            _player1 = player1;
            _player2 = player2;
        }

        public override void Initialize()
        {
            EventManager.Instance.RegisterListener<PlayerLostEvent>(this);

            _processManager = new ProcessManager();

            _mainCamera = new Camera(Constants.Global.WINDOW_WIDTH, Constants.Global.WINDOW_HEIGHT);
            _pongCamera = new PongCamera(Constants.Global.SCREEN_WIDTH, Constants.Global.SCREEN_HEIGHT);
            // The camera response to size changes
            EventManager.Instance.RegisterListener<ResizeEvent>(_mainCamera);

            _root = new Root(GameManager.GraphicsDevice.Viewport.Width,
                             GameManager.GraphicsDevice.Viewport.Height);
            _root.RegisterListeners();

            PresentationParameters pp = GameManager.GraphicsDevice.PresentationParameters;
            _pongRenderTarget = new RenderTarget2D(GameManager.GraphicsDevice,
                                                   Constants.Global.SCREEN_WIDTH,
                                                   Constants.Global.SCREEN_HEIGHT,
                                                   false,
                                                   SurfaceFormat.Color,
                                                   DepthFormat.None);

            VelocityParticleManager = new ParticleManager<VelocityParticleInfo>(1024 * 20, VelocityParticleInfo.UpdateParticle);

            _quadEffect = new BasicEffect(GameManager.GraphicsDevice);
            _quadEffect.AmbientLightColor = new Vector3(1, 1, 1);
            _quadEffect.World = Matrix.Identity;
            _quadEffect.TextureEnabled = true;
            _quad = new Quad(new Vector3(-Constants.Global.SCREEN_ASPECT_RATIO, 1, 0),
                                 new Vector3(Constants.Global.SCREEN_ASPECT_RATIO, 1, 0),
                                 new Vector3(-Constants.Global.SCREEN_ASPECT_RATIO, -1, 0),
                                 new Vector3(Constants.Global.SCREEN_ASPECT_RATIO, -1, 0),
                             Vector3.Forward);

            PongPostProcessor = new PostProcessor(GameManager.GraphicsDevice,
                                                  Constants.Global.SCREEN_WIDTH,
                                                  Constants.Global.SCREEN_HEIGHT);

            InitSystems();

            _directors = new BaseDirector[]{
                new PongDirector(this),
                new AstheticsDirector(this),
                new FluctuationDirector(this)
            };
            for (int i = 0; i < _directors.Length; i++)
            {
                _directors[i].RegisterEvents();
            }
        }

        void InitSystems()
        {
            _engine = new Engine();

            _aiSystem = new AIThinkSystem(_engine);
            _inputSystem = new InputSystem(_engine);
            _paddleSystem = new PaddleSystem(_engine);
            _ballMovementSystem = new BallMovementSystem(_engine);
            _goalSystem = new GoalSystem(_engine);
            _livesSystem = new LivesSystem(_engine);

            _renderSystem = new RenderSystem(GameManager.GraphicsDevice, _engine);

            _livesSystem.RegisterEventListeners();
        }

        public override void Hide()
        {

        }

        public override void LoadContent()
        {
            Content.Load<Texture2D>(Constants.Resources.TEXTURE_PONG_PADDLE);
            Content.Load<Texture2D>(Constants.Resources.TEXTURE_PONG_BALL);
            Content.Load<Texture2D>(Constants.Resources.TEXTURE_PONG_EDGE);
            Content.Load<Texture2D>(Constants.Resources.TEXTURE_PONG_GOAL);
            Content.Load<Texture2D>(Constants.Resources.TEXTURE_PONG_FIELD_BACKGROUND);

            Content.Load<Texture2D>(Constants.Resources.TEXTURE_PARTICLE_VELOCITY);

            Content.Load<Texture2D>(Constants.Resources.TEXTURE_BACKGROUND_BLACK);

            Content.Load<BitmapFont>(Constants.Resources.FONT_PONG_LIVES);
            Content.Load<BitmapFont>(Constants.Resources.FONT_PONG_INTRO);

            Content.Load<Effect>(Constants.Resources.EFFECT_WARP);
            Content.Load<Effect>(Constants.Resources.EFFECT_BLUR);
        }

        public override void Show()
        {
            CreateEntities();
            BuildUI();

            BeginIntroSequence();

            EventManager.Instance.TriggerEvent(new ResizeEvent(GameManager.GraphicsDevice.Viewport.Width,
                                                              GameManager.GraphicsDevice.Viewport.Height));
        }

        void CreateEntities()
        {
            EdgeEntity.Create(_engine, Content.Load<Texture2D>(Constants.Resources.TEXTURE_PONG_EDGE),
                              new Vector2(0, Constants.Pong.PLAYFIELD_HEIGHT / 2),
                              new Vector2(0, -1)); // Top edge points down
            EdgeEntity.Create(_engine, Content.Load<Texture2D>(Constants.Resources.TEXTURE_PONG_EDGE),
                              new Vector2(0, -Constants.Pong.PLAYFIELD_HEIGHT / 2),
                              new Vector2(0, 1)); // Bottom edge points up

            // Field background
            FieldBackgroundEntity.Create(_engine, Content.Load<Texture2D>(Constants.Resources.TEXTURE_PONG_FIELD_BACKGROUND));

            // Player 1 goal
            GoalEntity.Create(_engine, _player1, Content.Load<Texture2D>(Constants.Resources.TEXTURE_PONG_GOAL),
                              new Vector2(-Constants.Pong.PLAYFIELD_WIDTH / 2 + Constants.Pong.GOAL_WIDTH / 2, 0));
            // Player 2 goal
            GoalEntity.Create(_engine, _player2, Content.Load<Texture2D>(Constants.Resources.TEXTURE_PONG_GOAL),
                              new Vector2(Constants.Pong.PLAYFIELD_WIDTH / 2 - Constants.Pong.GOAL_WIDTH / 2, 0));

            Entity paddle1 = PaddleEntity.Create(_engine,
                                                 Content.Load<Texture2D>(Constants.Resources.TEXTURE_PONG_PADDLE),
                                                 new Vector2(-Constants.Pong.PADDLE_STARTING_X,
                                                             Constants.Pong.PADDLE_STARTING_Y),
                                                 new Vector2(1, 0)); // Left paddle normal points right
            Entity paddle2 = PaddleEntity.Create(_engine,
                                                 Content.Load<Texture2D>(Constants.Resources.TEXTURE_PONG_PADDLE),
                                                 new Vector2(Constants.Pong.PADDLE_STARTING_X,
                                                             Constants.Pong.PADDLE_STARTING_Y),
                                                 new Vector2(-1, 0)); // Right paddle normal points left

            paddle1.AddComponent(new PlayerComponent(_player1));
            paddle2.AddComponent(new PlayerComponent(_player2));

            if (Player1 is AIPlayer)
            {
                paddle1.AddComponent(new AIComponent(Player1 as AIPlayer));
            }
            if (Player2 is AIPlayer)
            {
                paddle2.AddComponent(new AIComponent(Player2 as AIPlayer));
            }

            // Lives
            LivesEntity.Create(_engine,
                               Content.Load<BitmapFont>(Constants.Resources.FONT_PONG_LIVES),
                               new Vector2(Constants.Pong.LIVES_LEFT_POSITION_X, Constants.Pong.LIVES_POSITION_Y),
                               _player1,
                               Constants.Pong.LIVES_COUNT);
            BallEntity.CreateWithoutBehavior(_engine,
                                             Content.Load<Texture2D>(Constants.Resources.TEXTURE_PONG_BALL),
                                             new Vector2(Constants.Pong.LIVES_ICON_LEFT_POSITION_X,
                                                         Constants.Pong.LIVES_POSITION_Y),
                                             2);
            LivesEntity.Create(_engine,
                               Content.Load<BitmapFont>(Constants.Resources.FONT_PONG_LIVES),
                               new Vector2(Constants.Pong.LIVES_RIGHT_POSITION_X, Constants.Pong.LIVES_POSITION_Y),
                               _player2,
                               Constants.Pong.LIVES_COUNT);
            BallEntity.CreateWithoutBehavior(_engine,
                                             Content.Load<Texture2D>(Constants.Resources.TEXTURE_PONG_BALL),
                                             new Vector2(Constants.Pong.LIVES_ICON_RIGHT_POSITION_X,
                                                         Constants.Pong.LIVES_POSITION_Y),
                                             2);
        }

        void BuildUI()
        {
            _pausedBackground = new Image(Content.Load<Texture2D>(Constants.Resources.TEXTURE_BACKGROUND_BLACK),
                                    Origin.TopLeft,
                                   0,
                                   0,
                                   0,
                                   0,
                                   1,
                                   0,
                                   1,
                                    (float)0);
            _pausedBackground.Hidden = true;
            _root.Add(_pausedBackground);

            _pausedLabel = new Label(Content.Load<BitmapFont>(Constants.Resources.FONT_PONG_INTRO),
                                     Origin.Center,
                                    0,
                                    0,
                                    0,
                                    0,
                                    0.25f,
                                    0,
                                     AspectRatioType.HeightMaster);
            _pausedLabel.Content = Constants.Pong.PAUSED_CONTENT;
            _pausedLabel.Hidden = true;
            _root.Add(_pausedLabel);
        }

        void BeginIntroSequence()
        {
            WaitProcess wait1 = new WaitProcess(Constants.Animations.INTRO_WAIT_DURATION);
            ReadyIntroTextAnimation readyIntroAnimation = new ReadyIntroTextAnimation(_engine,
                                                                                      FontEntity.Create(_engine,
                                                                                                        Vector2.Zero,
                                                                                                        Content.Load<BitmapFont>(Constants.Resources.FONT_PONG_INTRO),
                                                                                                        Constants.Pong.INTRO_READY_CONTENT),
                                                                                      _mainCamera,
                                                                                      GameManager.GraphicsDevice);
            wait1.SetNext(readyIntroAnimation);

            GoIntroTextAnimation goIntroAnimation = new GoIntroTextAnimation(_engine,
                                                                             FontEntity.Create(_engine,
                                                                                                        Vector2.Zero,
                                                                                                        Content.Load<BitmapFont>(Constants.Resources.FONT_PONG_INTRO),
                                                                                               Constants.Pong.INTRO_GO_CONTENT),
                                                                                      _mainCamera,
                                                                                      GameManager.GraphicsDevice);
            readyIntroAnimation.SetNext(goIntroAnimation);

            goIntroAnimation.SetNext(new DelegateCommand(() =>
            {
                EventManager.Instance.TriggerEvent(new StartEvent());
            }));

            _processManager.Attach(wait1);
        }

        void CheckPause()
        {
            if (_inputSystem.IsPauseButtonPressed())
            {
                if (_pauseKeyReleased)
                {
                    _pauseKeyReleased = false;

                    Paused = !Paused;
                }
            }
            else
            {
                _pauseKeyReleased = true;
            }
        }

        public override void Update(float dt)
        {
            _inputSystem.Update(dt);
            CheckPause();

            if (!Paused)
            {
                _processManager.Update(dt);

                // Do not need to be in lock-step
                _aiSystem.Update(dt);
                _livesSystem.Update(dt);
                _goalSystem.Update(dt);

                // Deterministic lock-step
                _acculmulator += dt;
                while (_acculmulator >= Constants.Global.TICK_RATE)
                {
                    _acculmulator -= Constants.Global.TICK_RATE;

                    _paddleSystem.Update(Constants.Global.TICK_RATE);
                    _ballMovementSystem.Update(Constants.Global.TICK_RATE);
                }

                // Update the directors
                for (int i = 0; i < _directors.Length; i++)
                {
                    _directors[i].Update(dt);
                }

                // Update particles
                VelocityParticleManager.Update(dt);

                // Update post-processing effects
                PongPostProcessor.Update(dt);
            }
        }

        public override void Draw(float dt)
        {
            float betweenFrameAlpha = _acculmulator / Constants.Global.TICK_RATE;

            DrawPong(dt, betweenFrameAlpha);
            DrawRemainder(dt, betweenFrameAlpha);

            _renderSystem.SpriteBatch.Begin();
            _root.Draw(_renderSystem.SpriteBatch);
            _renderSystem.SpriteBatch.End();
        }

        void DrawPong(float dt, float betweenFrameAlpha)
        {
            GameManager.GraphicsDevice.SetRenderTarget(_pongRenderTarget);
            Matrix center = Matrix.CreateTranslation(_pongRenderTarget.Bounds.Width / 2,
                                                     _pongRenderTarget.Bounds.Height / 2,
                                                     0);
            _renderSystem.DrawEntities(center,
                                       Constants.Pong.RENDER_GROUP,
                                        dt,
                                        betweenFrameAlpha);
            _renderSystem.SpriteBatch.Begin(SpriteSortMode.Deferred,
                                            null,
                                            null,
                                            null,
                                            null,
                                            null,
                                            center);
            VelocityParticleManager.Draw(_renderSystem.SpriteBatch);
            _renderSystem.SpriteBatch.Draw(Content.Load<Texture2D>(Constants.Resources.TEXTURE_PONG_EDGE),
                                           new Vector2(0, Constants.Pong.PLAYFIELD_HEIGHT / 2
                                                       + Constants.Pong.EDGE_HEIGHT
                                                      + 100 / 2),
                                          null,
                                           null,
                                           new Vector2(0.5f, 0.5f),
                                          0,
                                           new Vector2(Constants.Pong.PLAYFIELD_WIDTH,
                                                      100),
                                           Color.Black,
                                           SpriteEffects.None,
                                           0);
            _renderSystem.SpriteBatch.Draw(Content.Load<Texture2D>(Constants.Resources.TEXTURE_PONG_EDGE),
                                           new Vector2(0, -(Constants.Pong.PLAYFIELD_HEIGHT / 2
                                                       + Constants.Pong.EDGE_HEIGHT
                                                           + 100 / 2)),
                                          null,
                                           null,
                                           new Vector2(0.5f, 0.5f),
                                          0,
                                           new Vector2(Constants.Pong.PLAYFIELD_WIDTH,
                                                      100),
                                           Color.Black,
                                           SpriteEffects.None,
                                           0);
            _renderSystem.SpriteBatch.End();
            GameManager.GraphicsDevice.SetRenderTarget(null);

            _quadEffect.View = _pongCamera.TransformMatrix;
            _quadEffect.Projection = _pongCamera.PerspectiveMatrix;
            _quadEffect.Texture = _pongRenderTarget;

            PongPostProcessor.Begin();
            foreach (EffectPass pass in _quadEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                _quad.Draw(GameManager.GraphicsDevice);
            }

            RenderTarget2D processedPongRender = PongPostProcessor.End(false);

            _renderSystem.SpriteBatch.Begin(SpriteSortMode.Deferred,
                                           null,
                                           null,
                                           null,
                                           null,
                                           null,
                                            _mainCamera.TransformMatrix);
            _renderSystem.SpriteBatch.Draw(processedPongRender, new Rectangle((int)(processedPongRender.Bounds.X - processedPongRender.Bounds.Width / 2 + Constants.Pong.BUFFER_RENDER_POSITION.X),
                                                                              (int)(processedPongRender.Bounds.Y - processedPongRender.Bounds.Height / 2 + Constants.Pong.BUFFER_RENDER_POSITION.Y),
                                                                              processedPongRender.Bounds.Width,
                                                                              processedPongRender.Bounds.Height), Color.White);
            _renderSystem.SpriteBatch.End();
        }

        void DrawRemainder(float dt, float betweenFrameAlpha)
        {
            // Render everything else (everything not pong)
            _renderSystem.DrawEntities(_mainCamera.TransformMatrix,
                                        (byte)(Constants.Render.GROUP_MASK_ALL & ~Constants.Pong.RENDER_GROUP),
                                        dt,
                                        betweenFrameAlpha);
        }

        public override void Dispose()
        {
            // Remove listeners
            EventManager.Instance.UnregisterListener(this);
            EventManager.Instance.UnregisterListener(_mainCamera);
            _livesSystem.UnregisterEventListeners();
            for (int i = 0; i < _directors.Length; i++)
            {
                _directors[i].UnregisterEvents();
            }
            _root.UnregisterListeners();

            PongPostProcessor.Dispose();
        }

        public bool Handle(IEvent evt)
        {
            PlayerLostEvent playerLostEvent = evt as PlayerLostEvent;
            if (playerLostEvent != null)
            {
                WaitProcess processChain = new WaitProcess(Constants.Animations.GAME_OVER_WAIT);
                string gameOverContent = playerLostEvent.WinPlayer.Name + Constants.Pong.GAME_OVER_CONTENT_SUFFIX;
                processChain.SetNext(new GameOverTextAnimation(_engine,
                                                       FontEntity.Create(_engine,
                                                                         Vector2.Zero,
                                                                        Content.Load<BitmapFont>(Constants.Resources.FONT_PONG_INTRO),
                                                                         gameOverContent),
                                                       _mainCamera,
                                                       GameManager.GraphicsDevice))
                            .SetNext(new WaitProcess(Constants.Animations.GAME_OVER_POST_WAIT))
                            .SetNext(new DelegateCommand(() =>
                            {
                                GameManager.ChangeState(new LobbyGameState(GameManager,
                                                                           _player1.InputMethod,
                                                                           (_player2 is AIPlayer) ? null : _player2.InputMethod));
                            }));

                _processManager.Attach(processChain);

                return false;
            }

            return false;
        }
    }
}

