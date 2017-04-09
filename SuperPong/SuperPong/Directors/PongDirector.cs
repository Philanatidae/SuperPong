using System;
using ECS;
using Events;
using Microsoft.Xna.Framework;
using SuperPong.Events;

namespace SuperPong.Directors
{
	public class PongDirector : IEventListener
	{
		readonly Engine _engine;

		public PongDirector(Engine engine)
		{
			_engine = engine;
		}

		public void RegisterEvents()
		{
		}

		public void UnregisterEvents()
		{
			EventManager.Instance.UnregisterListener(this);
		}

		public bool Handle(IEvent evt)
		{
			return false;
		}

		public void Update(GameTime gameTime)
		{
		}
	}
}
