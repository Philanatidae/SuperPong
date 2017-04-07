using ECS;
using Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperPong.Common;
using SuperPong.Components;
using SuperPong.Entities;
using SuperPong.Events;
using SuperPong.Input;
using SuperPong.Systems;

namespace SuperPong
{
	public class MainGameState : GameState
	{
		InputMethod _player1InputMethod;
		InputMethod _player2InputMethod;

		float _acculmulator;

		Engine _engine;
		InputSystem _inputSystem;
		PaddleSystem _paddleSystem;
		BallMovementSystem _ballMovementSystem;
		RenderSystem _renderSystem;

		Camera _mainCamera;
		Camera _pongCamera;

		RenderTarget2D _pongRenderTarget;

		public MainGameState(GameManager gameManager,
		                     InputMethod player1InputMethod,
		                     InputMethod player2InputMethod)
			:base(gameManager)
		{
			_player1InputMethod = player1InputMethod;
			_player2InputMethod = player2InputMethod;
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
		}

		void InitSystems()
		{
			_engine = new Engine();

			_inputSystem = new InputSystem(_engine);
			_paddleSystem = new PaddleSystem(_engine);
			_ballMovementSystem = new BallMovementSystem(_engine);

			_renderSystem = new RenderSystem(GameManager.GraphicsDevice, _engine);
		}

		public override void Hide()
		{
			
		}

		public override void LoadContent()
		{
			Content.Load<Texture2D>(Constants.Resources.TEXTURE_PONG_BALL);
			Content.Load<Texture2D>(Constants.Resources.TEXTURE_PONG_PADDLE);
			Content.Load<Texture2D>(Constants.Resources.TEXTURE_PONG_EDGE);
		}

		public override void Show()
		{
			CreateEntities();
		}

		void CreateEntities()
		{
			EdgeEntity.Create(_engine, Content.Load<Texture2D>(Constants.Resources.TEXTURE_PONG_EDGE), true);
			EdgeEntity.Create(_engine, Content.Load<Texture2D>(Constants.Resources.TEXTURE_PONG_EDGE), false);

			BallEntity.Create(_engine, Content.Load<Texture2D>(Constants.Resources.TEXTURE_PONG_BALL), Vector2.Zero);

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

			paddle1.AddComponent(new InputComponent(_player1InputMethod));
			paddle2.AddComponent(new InputComponent(_player2InputMethod));
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
			}
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
			                               null,
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
		}
	}
}
