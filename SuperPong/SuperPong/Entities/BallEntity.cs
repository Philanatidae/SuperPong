using ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperPong.Components;

namespace SuperPong.Entities
{
	public class BallEntity
	{
		public static Entity Create(Engine engine, Texture2D texture, Vector2 position)
		{
			Entity entity = engine.CreateEntity();

			entity.AddComponent(new TransformComponent(position));
			entity.AddComponent(new SpriteComponent(texture,
													new Vector2(Constants.Pong.BALL_WIDTH,
			                                                    Constants.Pong.BALL_HEIGHT)));
			entity.AddComponent(new BallComponent());

			return entity;
		}
	}
}
