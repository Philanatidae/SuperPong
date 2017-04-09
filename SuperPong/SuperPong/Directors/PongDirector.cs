using ECS;
using Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
			EventManager.Instance.RegisterListener<GoalEvent>(this);
		}

		public void UnregisterEvents()
		{
			EventManager.Instance.UnregisterListener(this);
		}

		public bool Handle(IEvent evt)
		{
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
		public void HandleGoal(GoalEvent goalEvent)
		{
			_owner.Engine.DestroyEntity(goalEvent.Ball);
			_processManager.Attach(new CreateBall(_owner.Engine, _owner.BallTexture));
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
