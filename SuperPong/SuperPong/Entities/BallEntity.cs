using ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperPong.Components;

namespace SuperPong.Entities
{
	public static class BallEntity
	{
		public static Entity Create(Engine engine, Texture2D texture, Vector2 position, float direction)
		{
			Entity entity = engine.CreateEntity();

			entity.AddComponent(new TransformComponent(position));
			entity.AddComponent(new BallComponent(direction));

			// Link the bounds in BallComponent to SpriteComponent
			entity.AddComponent(new SpriteComponent(texture, entity.GetComponent<BallComponent>().Bounds));
			entity.GetComponent<SpriteComponent>().RenderGroup = Constants.Pong.RENDER_GROUP;

			return entity;
		}
	}
}
