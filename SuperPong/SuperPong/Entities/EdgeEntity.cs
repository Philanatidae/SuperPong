using ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperPong.Components;

namespace SuperPong.Entities
{
	public class EdgeEntity
	{
		public static Entity Create(Engine engine, Texture2D texture, bool isTop)
		{
			Entity entity = engine.CreateEntity();

			float yPos = Constants.Pong.PLAYFIELD_HEIGHT / 2;
			yPos += Constants.Pong.EDGE_HEIGHT / 2;
			yPos *= (isTop ? 1 : -1);

			entity.AddComponent(new TransformComponent(new Vector2(0, yPos)));
			entity.AddComponent(new SpriteComponent(texture, new Vector2(Constants.Pong.PLAYFIELD_WIDTH,
			                                                             Constants.Pong.EDGE_HEIGHT)));
			entity.AddComponent(new EdgeComponent());

			return entity;
		}
	}
}
