using ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperPong.Components;

namespace SuperPong.Entities
{
	public static class GoalEntity
	{
		public static Entity Create(Engine engine, Player forPlayer, Texture2D texture, Vector2 position)
		{
			Entity entity = engine.CreateEntity();

			entity.AddComponent(new TransformComponent(position));
			entity.AddComponent(new SpriteComponent(texture, new Vector2(Constants.Pong.GOAL_WIDTH,
			                                                             Constants.Pong.GOAL_HEIGHT)));
			entity.GetComponent<SpriteComponent>().RenderGroup = Constants.Pong.RENDER_GROUP;
			entity.AddComponent(new GoalComponent(forPlayer));

			return entity;
		}
	}
}
