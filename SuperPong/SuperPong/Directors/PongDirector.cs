using ECS;
using Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperPong.Components;
using SuperPong.Events;
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

			return false;
		}

		public void Update(GameTime gameTime)
		{
			_processManager.Update(gameTime);
		}

		// HANDLERS!
		void HandleStart(StartEvent startEvent)
		{
			_processManager.Attach(new CreateBall(_owner.Engine, _owner.BallTexture,
			                                      Constants.Pong.BALL_PLAYER1_STARTING_ROTATION_DEGREES));
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
			}

			ballReturnSequence.SetNext(new CreateBall(_owner.Engine, _owner.BallTexture, direction));
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
	}

	public interface IPongDirectorOwner
	{
		Engine Engine
		{
			get;
		}

		Texture2D BallTexture
		{
			get;
		}
	}
}
