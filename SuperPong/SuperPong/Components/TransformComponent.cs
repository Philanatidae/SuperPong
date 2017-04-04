using ECS;
using Microsoft.Xna.Framework;

namespace SuperPong.Components
{
	public class TransformComponent : IComponent
	{
		public TransformComponent()
		{
		}

		public TransformComponent(Vector2 position)
		{
			this.position = position;
		}

		public TransformComponent(Vector2 position, float rotation)
		{
			this.rotation = rotation;
		}

		public Vector2 position;
		public float rotation;
	}
}
