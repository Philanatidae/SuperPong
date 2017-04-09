using ECS;
using Microsoft.Xna.Framework;

namespace SuperPong.Components
{
	public class TransformComponent : IComponent
	{
		public TransformComponent()
		{
		}

		public TransformComponent(Vector2 position) :this(position, 0)
		{
		}

		public TransformComponent(Vector2 position, float rotation)
		{
			Position = position;
			Rotation = rotation;
		}

		public Vector2 Position;
		public float Rotation;
	}
}
