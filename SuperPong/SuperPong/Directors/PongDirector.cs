using System;
using ECS;
using Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperPong.Components;
using SuperPong.Events;
using SuperPong.Fluctuations;
using SuperPong.Processes;
using SuperPong.Processes.Pong;

namespace SuperPong.Directors
{
	public class PongDirector : IEventListener
	{
		readonly IPongDirectorOwner _owner;

		ProcessManager _processManager = new ProcessManager();

		readonly Family _ballFamily = Family.All(typeof(BallComponent), typeof(TransformComponent)).Get();

		readonly ImmutableList<Entity> _ballEntities;

		readonly Random _random = new Random();

		int player1Lives = Constants.Pong.LIVES_COUNT;
		int player2Lives = Constants.Pong.LIVES_COUNT;

		readonly Timer _fluctuationTimer = new Timer(0);

		public PongDirector(IPongDirectorOwner owner)
		{
			_owner = owner;

			_ballEntities = _owner.Engine.GetEntitiesFor(_ballFamily);
		}

		public void RegisterEvents()
		{
			EventManager.Instance.RegisterListener<StartEvent>(this);
			EventManager.Instance.RegisterListener<GoalEvent>(this);
			EventManager.Instance.RegisterListener<BallBounceEvent>(this);
			EventManager.Instance.RegisterListener<FluctuationEndEvent>(this);
		}

		public void UnregisterEvents()
		{
			EventManager.Instance.UnregisterListener(this);
		}

		public bool Handle(IEvent evt)
		{
			if (evt is StartEvent)
			{
				HandleStart(evt as StartEvent);
			}
			if (evt is GoalEvent)
			{
				HandleGoal(evt as GoalEvent);
			}
			if (evt is BallBounceEvent)
			{
				HandleBallBounce(evt as BallBounceEvent);
			}
			if (evt is FluctuationEndEvent)
			{
				HandleFluctuationEnd(evt as FluctuationEndEvent);
			}

			return false;
		}

		public void Update(GameTime gameTime)
		{
			_processManager.Update(gameTime);

			_fluctuationTimer.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
			if (_fluctuationTimer.HasElapsed())
			{
				AttachRandomFluctuation();
			}
		}

		void ResetFluctuationTimer()
		{
			float duration = (float)(_random.NextDouble()
									* (Constants.Fluctuations.TIMER_MAX - Constants.Fluctuations.TIMER_MIN)
									+ Constants.Fluctuations.TIMER_MIN);
			_fluctuationTimer.Reset(duration);
			_fluctuationTimer.Enabled = true;
		}

		void AttachRandomFluctuation()
		{
			int id = _random.Next(1, Constants.Fluctuations.FLUCTUATIONS_COUNT);
			switch (id)
			{
				case 1:
					_processManager.Attach(new WarpFluctuation(_owner));
					break;
					default:
					throw new NotImplementedException();
			}
			_fluctuationTimer.Enabled = false;
		}

		// HANDLERS!
		void HandleStart(StartEvent startEvent)
		{
			_processManager.Attach(new CreateBall(_owner.Engine, _owner.BallTexture,
			                                      Constants.Pong.BALL_PLAYER1_STARTING_ROTATION_DEGREES));


			ResetFluctuationTimer();
		}

		void HandleGoal(GoalEvent goalEvent)
		{
			_owner.Engine.DestroyEntity(goalEvent.Ball);

			Process ballReturnSequence = new WaitProcess(1.0f);
			_processManager.Attach(ballReturnSequence);

			float direction = Constants.Pong.BALL_PLAYER2_STARTING_ROTATION_DEGREES;
			GoalComponent goalComp = goalEvent.Goal.GetComponent<GoalComponent>();
			if (goalComp.For.Index == 0)
			{
				direction = Constants.Pong.BALL_PLAYER1_STARTING_ROTATION_DEGREES;
				player1Lives--;
			}
			else
			{
				player2Lives--;
			}

			if (player1Lives <= 0 || player2Lives <= 0)
			{
				// Lost
				if (player1Lives <= 0)
				{
					EventManager.Instance.QueueEvent(new PlayerLostEvent(_owner.Player1, _owner.Player2));
				}
				else
				{
					EventManager.Instance.QueueEvent(new PlayerLostEvent(_owner.Player2, _owner.Player1));
				}
			}
			else
			{
				ballReturnSequence.SetNext(new CreateBall(_owner.Engine, _owner.BallTexture, direction));
			}
		}

		void HandleBallBounce(BallBounceEvent ballBounceEvent)
		{
			// Only if it is a paddle
			if (ballBounceEvent.Collider.HasComponent<PaddleComponent>())
			{
				foreach (Entity ballEntity in _ballEntities)
				{
					BallComponent ballComp = ballEntity.GetComponent<BallComponent>();
					ballComp.Velocity += Constants.Pong.BALL_SPEED_INCREASE;
				}
			}
		}

		void HandleFluctuationEnd(FluctuationEndEvent fluctuationEndEvent)
		{
			ResetFluctuationTimer();
		}
	}

	public interface IPongDirectorOwner
	{
		Engine Engine
		{
			get;
		}

		Effect PongRenderEffect
		{
			get;
			set;
		}

		Texture2D BallTexture
		{
			get;
		}

		Effect WarpEffect
		{
			get;
		}

		Player Player1
		{
			get;
		}

		Player Player2
		{
			get;
		}
	}
}
