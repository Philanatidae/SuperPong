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

		public PongDirector(IPongDirectorOwner owner)
		{
			_owner = owner;
		}

		public void RegisterEvents()
		{
			EventManager.Instance.RegisterListener<StartEvent>(this);
			EventManager.Instance.RegisterListener<GoalEvent>(this);
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
