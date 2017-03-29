using Microsoft.Xna.Framework;

namespace ECS
{
	public abstract class EntitySystem
	{
		readonly Engine _engine;

		public EntitySystem(Engine engine)
		{
			_engine = engine;
		}

		public abstract void Update(GameTime gameTime);

		public Engine getEngine()
		{
			return _engine;
		}

	}
}
