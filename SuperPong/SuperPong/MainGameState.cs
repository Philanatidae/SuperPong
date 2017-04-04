using System;
using ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperPong.Common;
using SuperPong.Components;
using SuperPong.Entities;
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
			_renderSystem.Draw(_mainCamera.TransformMatrix, gameTime);
		}
	}
}
