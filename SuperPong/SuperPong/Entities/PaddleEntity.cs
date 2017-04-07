using ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperPong.Components;

namespace SuperPong.Entities
{
	public class PaddleEntity
	{
		public static Entity Create(Engine engine, Texture2D texture, Vector2 position, Vector2 normal)
		{
			Entity entity = engine.CreateEntity();

			entity.AddComponent(new TransformComponent(position));
			entity.AddComponent(new SpriteComponent(texture, new Vector2(Constants.Pong.PADDLE_WIDTH,
																		 Constants.Pong.PADDLE_HEIGHT)));
			entity.GetComponent<SpriteComponent>().RenderGroup = Constants.Pong.RENDER_GROUP;
			entity.AddComponent(new PaddleComponent(normal));

			return entity;
		}
	}
}
