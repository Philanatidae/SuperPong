using System;
using ECS;
using Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperPong.Common;
using SuperPong.Components;
using SuperPong.Directors;
using SuperPong.Entities;
using SuperPong.Events;
using SuperPong.Input;
using SuperPong.Systems;

namespace SuperPong
{
	public class MainGameState : GameState, IPongDirectorOwner
	{
		Player _player1;
		Player _player2;

		float _acculmulator;

		Engine _engine;
		InputSystem _inputSystem;
		PaddleSystem _paddleSystem;
		BallMovementSystem _ballMovementSystem;
		GoalSystem _goalSystem;
		RenderSystem _renderSystem;

		PongDirector _director;

		Texture2D _paddleTexture;
		Texture2D _ballTexture;
		Texture2D _edgeTexture;
		Texture2D _goalTexture;

		Camera _mainCamera;
		Camera _pongCamera;

		RenderTarget2D _pongRenderTarget;

		public Engine Engine
		{
			get
			{
				return _engine;
			}
		}

		public Texture2D BallTexture
		{
			get
			{
				return _ballTexture;
			}
		}

		public MainGameState(GameManager gameManager,
		                     Player player1,
		                     Player player2)
			:base(gameManager)
		{
			_player1 = player1;
			_player2 = player2;
		}

		public override void Initialize()
		{
			_mainCamera = new Camera(GameManager.GraphicsDevice.Viewport);
			_pongCamera = new Camera(GameManager.GraphicsDevice.Viewport);
			// The camera response to size changes
			EventManager.Instance.RegisterListener<ResizeEvent>(_mainCamera);

			PresentationParameters pp = GameManager.GraphicsDevice.PresentationParameters;
			_pongRenderTarget = new RenderTarget2D(GameManager.GraphicsDevice,
												   pp.BackBufferWidth,
												   pp.BackBufferHeight,
												   true,
			                                       SurfaceFormat.Color,
			                                       DepthFormat.None);

			InitSystems();

			_director = new PongDirector(this);
			_director.RegisterEvents();
		}

		void InitSystems()
		{
			_engine = new Engine();

			_inputSystem = new InputSystem(_engine);
			_paddleSystem = new PaddleSystem(_engine);
			_ballMovementSystem = new BallMovementSystem(_engine);
			_goalSystem = new GoalSystem(_engine);

			_renderSystem = new RenderSystem(GameManager.GraphicsDevice, _engine);
		}

		public override void Hide()
		{
			
		}

		public override void LoadContent()
		{
			_paddleTexture = Content.Load<Texture2D>(Constants.Resources.TEXTURE_PONG_PADDLE);
			_ballTexture = Content.Load<Texture2D>(Constants.Resources.TEXTURE_PONG_BALL);
			_edgeTexture = Content.Load<Texture2D>(Constants.Resources.TEXTURE_PONG_EDGE);
			_goalTexture = Content.Load<Texture2D>(Constants.Resources.TEXTURE_PONG_GOAL);
		}

		public override void Show()
		{
			CreateEntities();
			EventManager.Instance.TriggerEvent(new StartEvent());
		}

		void CreateEntities()
		{
			EdgeEntity.Create(_engine, _edgeTexture,
							  new Vector2(0, Constants.Pong.PLAYFIELD_HEIGHT / 2),
			                  new Vector2(0, -1)); // Top edge points down
			EdgeEntity.Create(_engine, _edgeTexture,
			                  new Vector2(0, -Constants.Pong.PLAYFIELD_HEIGHT / 2),
			                  new Vector2(0, 1)); // Bottom edge points up

			// Player 1 goal
			GoalEntity.Create(_engine, _player1, _goalTexture,
			                  new Vector2(-Constants.Pong.PLAYFIELD_WIDTH / 2 + Constants.Pong.GOAL_WIDTH / 2, 0));
			// Player 2 goal
			GoalEntity.Create(_engine, _player2, _goalTexture,
			                  new Vector2(Constants.Pong.PLAYFIELD_WIDTH / 2 - Constants.Pong.GOAL_WIDTH / 2, 0));

			Entity paddle1 = PaddleEntity.Create(_engine,
			                                     	_paddleTexture,
			                                         new Vector2(-Constants.Pong.PADDLE_STARTING_X,
			                                                     Constants.Pong.PADDLE_STARTING_Y),
			                                         new Vector2(1, 0)); // Left paddle normal points right
			Entity paddle2 = PaddleEntity.Create(_engine,
                                 _paddleTexture,
								 new Vector2(Constants.Pong.PADDLE_STARTING_X,
											 Constants.Pong.PADDLE_STARTING_Y),
								 new Vector2(-1, 0)); // Right paddle normal points left

			paddle1.AddComponent(new PlayerComponent(_player1));
			paddle2.AddComponent(new PlayerComponent(_player2));
		}

		public override void Update(GameTime gameTime)
		{
			_acculmulator += (float)gameTime.ElapsedGameTime.TotalSeconds;

			while (_acculmulator >= Constants.Global.TICK_RATE)
			{
				_acculmulator -= Constants.Global.TICK_RATE;

				_inputSystem.Update(Constants.Global.TICK_RATE);

				_paddleSystem.Update(Constants.Global.TICK_RATE);
				_ballMovementSystem.Update(Constants.Global.TICK_RATE);
				_goalSystem.Update(Constants.Global.TICK_RATE);
			}

			_director.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			DrawPong(gameTime);
			DrawRemainder(gameTime);
		}

		void DrawPong(GameTime gameTime)
		{
			GameManager.GraphicsDevice.SetRenderTarget(_pongRenderTarget);
			_renderSystem.DrawEntities(_pongCamera.TransformMatrix,
							   Constants.Pong.RENDER_GROUP,
							   gameTime);
			GameManager.GraphicsDevice.SetRenderTarget(null);

			_renderSystem.SpriteBatch.Begin(SpriteSortMode.Deferred,
			                               null,
			                                SamplerState.PointWrap,
			                               null,
			                               null,
			                               null,
			                                _mainCamera.TransformMatrix);
			_renderSystem.SpriteBatch.Draw(_pongRenderTarget,
			                               Constants.Pong.BUFFER_RENDER_POSITION * RenderSystem.FlipY,
										   null,
										   Color.White,
										   0,
			                               new Vector2(_pongRenderTarget.Width / 2,
			                                           _pongRenderTarget.Height / 2),
										   Vector2.One,
										   SpriteEffects.None,
										   0);
			_renderSystem.SpriteBatch.End();
		}

		void DrawRemainder(GameTime gameTime)
		{
			// Render everything else (everything not pong)
			_renderSystem.DrawEntities(_mainCamera.TransformMatrix,
							   (byte)(Constants.Render.GROUP_MASK_ALL & ~Constants.Pong.RENDER_GROUP),
							   gameTime);
		}

		public override void Dispose()
		{
			// Remove listeners
			EventManager.Instance.UnregisterListener(_mainCamera);
			_director.UnregisterEvents();
		}
	}
}
