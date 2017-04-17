using System;
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
		LivesSystem _livesSystem;
		RenderSystem _renderSystem;

		PongDirector _director;

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

		public Effect PongRenderEffect
		{
			get;
			set;
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

			Content.Load<BitmapFont>(Constants.Resources.FONT_PONG_LIVES);

			Content.Load<Effect>(Constants.Resources.EFFECT_WARP);
		}

		public override void Show()
		{
			CreateEntities();
			EventManager.Instance.TriggerEvent(new StartEvent());
		}

		void CreateEntities()
		{
			EdgeEntity.Create(_engine, Content.Load<Texture2D>(Constants.Resources.TEXTURE_PONG_EDGE),
							  new Vector2(0, Constants.Pong.PLAYFIELD_HEIGHT / 2),
			                  new Vector2(0, -1)); // Top edge points down
			EdgeEntity.Create(_engine, Content.Load<Texture2D>(Constants.Resources.TEXTURE_PONG_EDGE),
			                  new Vector2(0, -Constants.Pong.PLAYFIELD_HEIGHT / 2),
			                  new Vector2(0, 1)); // Bottom edge points up

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
				_livesSystem.Update(Constants.Global.TICK_RATE);
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
			                                PongRenderEffect,
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
			_livesSystem.UnregisterEventListeners();
			_director.UnregisterEvents();
		}
	}
}
