using ECS;
using Microsoft.Xna.Framework;

namespace SuperPong.Components
{
	public class TransformComponent : IComponent
	{
		public Vector2 position;
		public float rotation;
	}
}
