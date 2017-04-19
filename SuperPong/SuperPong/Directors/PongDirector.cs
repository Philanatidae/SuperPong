using System;
using ECS;
using Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
		enum DirectorState
		{
			WaitingToStart,
			InProgress,
			WaitingForFluctuationEnd,
			GameOver
		}

		readonly IPongDirectorOwner _owner;

		ProcessManager _processManager = new ProcessManager();
		readonly Random _random = new Random();

		readonly Family _ballFamily = Family.All(typeof(BallComponent), typeof(TransformComponent)).Get();
		readonly ImmutableList<Entity> _ballEntities;

		readonly Timer _fluctuationTimer = new Timer(0);

		DirectorState _state = DirectorState.WaitingToStart;

		int player1Lives = Constants.Pong.LIVES_COUNT;
		int player2Lives = Constants.Pong.LIVES_COUNT;

		public PongDirector(IPongDirectorOwner owner)
		{
			_owner = owner;

			_ballEntities = _owner.Engine.GetEntitiesFor(_ballFamily);

			_fluctuationTimer.Enabled = false;
		}

		public void RegisterEvents()
		{
			EventManager.Instance.RegisterListener<StartEvent>(this);
			EventManager.Instance.RegisterListener<GoalEvent>(this);
			EventManager.Instance.RegisterListener<BallBounceEvent>(this);
			EventManager.Instance.RegisterListener<FluctuationEndEvent>(this);
			EventManager.Instance.RegisterListener<PlayerLostEvent>(this);
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
			if (evt is PlayerLostEvent)
			{
				HandlePlayerLost(evt as PlayerLostEvent);
			}

			return false;
		}

		public void Update(GameTime gameTime)
		{
			_processManager.Update(gameTime);

			switch (_state)
			{
				case DirectorState.InProgress:
					_fluctuationTimer.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
					if (_fluctuationTimer.HasElapsed())
					{
						AttachRandomFluctuation();
					}
					break;
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

		void PlayNewBall(Player playerToServe)
		{
			Process ballReturnSequence = new WaitProcess(1.0f);
			_processManager.Attach(ballReturnSequence);

			float direction = Constants.Pong.BALL_PLAYER2_STARTING_ROTATION_DEGREES;
			if (playerToServe == _owner.Player1)
			{
				direction = Constants.Pong.BALL_PLAYER1_STARTING_ROTATION_DEGREES;
			}

			_state = DirectorState.InProgress;

			ballReturnSequence.SetNext(new CreateBall(_owner.Engine, _owner.Content.Load<Texture2D>(Constants.Resources.TEXTURE_PONG_BALL), direction))
			                  .SetNext(new DelegateCommand(() =>
							  {
								  ResetFluctuationTimer();
							  }));
		}

		// GETTERS!
		bool IsFluctuationAttached()
		{
			return GetCurrentFluctuation() != null;
		}

		Fluctuation GetCurrentFluctuation()
		{
			foreach (Process process in _processManager.Processes)
			{
				Fluctuation fluctuation = process as Fluctuation;
				if (fluctuation != null)
				{
					return fluctuation;
				}
			}

			return null;
		}

		void EndCurrentFluctuation()
		{
			Fluctuation runningFluctuation = GetCurrentFluctuation();
			if (runningFluctuation != null)
			{
				runningFluctuation.SoftEnd();
			}
		}

		// HANDLERS!
		void HandleStart(StartEvent startEvent)
		{
			_state = DirectorState.InProgress;

			PlayNewBall(_owner.Player1);
		}

		void HandleGoal(GoalEvent goalEvent)
		{
			GoalComponent goalComp = goalEvent.Goal.GetComponent<GoalComponent>();

			_owner.Engine.DestroyEntity(goalEvent.Ball);

			if (goalComp.For.Index == 0)
			{
				if (--player1Lives <= 0)
				{
					EventManager.Instance.QueueEvent(new PlayerLostEvent(_owner.Player1, _owner.Player2));
					EndCurrentFluctuation();
					return;
				}
			}
			if (goalComp.For.Index == 1)
			{
				if (--player2Lives <= 0)
				{
					EventManager.Instance.QueueEvent(new PlayerLostEvent(_owner.Player2, _owner.Player1));
					EndCurrentFluctuation();
					return;
				}
			}

			// Queue processes for next ball serve
			Process ballPlayProcess = new DelegateCommand(() =>
			{
				if (goalComp.For.Index == 0)
				{
					PlayNewBall(_owner.Player1);
				}
				if (goalComp.For.Index == 1)
				{
					PlayNewBall(_owner.Player2);
				}
			});

			// If there is a fluctuation, attach the serve to the fluctuation's end
			if (IsFluctuationAttached())
			{
				EndCurrentFluctuation();
				_state = DirectorState.WaitingForFluctuationEnd;

				WaitForEvent fluctuationEnd = new WaitForEvent(typeof(FluctuationEndEvent));
				fluctuationEnd.SetNext(ballPlayProcess);
				_processManager.Attach(fluctuationEnd);
				return;
			}

			// Attach the serve
			_processManager.Attach(ballPlayProcess);
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
			if(_state == DirectorState.InProgress)
			{
				ResetFluctuationTimer();
			}
		}

		void HandlePlayerLost(PlayerLostEvent playerLost)
		{
			_state = DirectorState.GameOver;
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

		ContentManager Content
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
