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

namespace SuperPong
{
    public class MainGameState : GameState, IPongDirectorOwner
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

        PongDirector _director;

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
            _processManager = new ProcessManager();

            _mainCamera = new Camera(GameManager.GraphicsDevice.Viewport);
            _pongCamera = new PongCamera();
            // The camera response to size changes
            EventManager.Instance.RegisterListener<ResizeEvent>(_mainCamera);
            EventManager.Instance.RegisterListener<ResizeEvent>(_pongCamera);

            PresentationParameters pp = GameManager.GraphicsDevice.PresentationParameters;
            _pongRenderTarget = new RenderTarget2D(GameManager.GraphicsDevice,
                                                   pp.BackBufferWidth,
                                                   pp.BackBufferHeight,
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

            PongPostProcessor = new PostProcessor(GameManager.GraphicsDevice);
            EventManager.Instance.RegisterListener<ResizeEvent>(PongPostProcessor);

            InitSystems();

            _director = new PongDirector(this);
            _director.RegisterEvents();
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

            Content.Load<BitmapFont>(Constants.Resources.FONT_PONG_LIVES);
            Content.Load<BitmapFont>(Constants.Resources.FONT_PONG_INTRO);

            Content.Load<Effect>(Constants.Resources.EFFECT_WARP);
            Content.Load<Effect>(Constants.Resources.EFFECT_BLUR);
        }

        public override void Show()
        {
            CreateEntities();

            BeginIntroSequence();
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

        void BeginIntroSequence()
        {
            WaitProcess wait1 = new WaitProcess(Constants.Animations.INTRO_WAIT_DURATION);
            ReadyIntroTextAnimation readyIntroAnimation = new ReadyIntroTextAnimation(_engine,
                                                                                      FontEntity.Create(_engine,
                                                                                                        Vector2.Zero,
                                                                                                        Content.Load<BitmapFont>(Constants.Resources.FONT_PONG_INTRO),
                                                                                                        Constants.Pong.INTRO_READY_CONTENT),
                                                                                      _mainCamera,
                                                                                      GameManager.GraphicsDevice.Viewport);
            wait1.SetNext(readyIntroAnimation);

            GoIntroTextAnimation goIntroAnimation = new GoIntroTextAnimation(_engine,
                                                                             FontEntity.Create(_engine,
                                                                                                        Vector2.Zero,
                                                                                                        Content.Load<BitmapFont>(Constants.Resources.FONT_PONG_INTRO),
                                                                                               Constants.Pong.INTRO_GO_CONTENT),
                                                                                      _mainCamera,
                                                                                      GameManager.GraphicsDevice.Viewport);
            readyIntroAnimation.SetNext(goIntroAnimation);

            goIntroAnimation.SetNext(new DelegateCommand(() =>
            {
                EventManager.Instance.TriggerEvent(new StartEvent());
            }));

            _processManager.Attach(wait1);
        }

        public override void Update(float dt)
        {
            _processManager.Update(dt);

            // Do not need to be in lock-step
            _aiSystem.Update(dt);
            _inputSystem.Update(dt);
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

            // Update the director
            _director.Update(dt);

            // Update particles
            VelocityParticleManager.Update(dt);

            // Update post-processing effects
            PongPostProcessor.Update(dt);
        }

        public override void Draw(float dt)
        {
            float betweenFrameAlpha = _acculmulator / Constants.Global.TICK_RATE;

            DrawPong(dt, betweenFrameAlpha);
            DrawRemainder(dt, betweenFrameAlpha);
        }

        void DrawPong(float dt, float betweenFrameAlpha)
        {
            GameManager.GraphicsDevice.SetRenderTarget(_pongRenderTarget);
            _renderSystem.DrawEntities(Matrix.CreateTranslation(GameManager.GraphicsDevice.Viewport.Width / 2 + Constants.Pong.BUFFER_RENDER_POSITION.X,
                                                                GameManager.GraphicsDevice.Viewport.Height / 2 + Constants.Pong.BUFFER_RENDER_POSITION.Y,
                                                               0),
                                        Constants.Pong.RENDER_GROUP,
                                        dt,
                                        betweenFrameAlpha);
            _renderSystem.SpriteBatch.Begin(SpriteSortMode.Deferred,
                                            null,
                                            null,
                                            null,
                                            null,
                                            null,
                                           Matrix.CreateTranslation(GameManager.GraphicsDevice.Viewport.Width / 2 + Constants.Pong.BUFFER_RENDER_POSITION.X,
                                                                GameManager.GraphicsDevice.Viewport.Height / 2 + Constants.Pong.BUFFER_RENDER_POSITION.Y,
                                                               0));
            VelocityParticleManager.Draw(_renderSystem.SpriteBatch);
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

            PongPostProcessor.End();
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
            EventManager.Instance.UnregisterListener(_mainCamera);
            EventManager.Instance.UnregisterListener(_pongCamera);
            _livesSystem.UnregisterEventListeners();
            _director.UnregisterEvents();

            EventManager.Instance.UnregisterListener(PongPostProcessor);
            PongPostProcessor.Dispose();
        }
    }
}

