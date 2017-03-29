using ECS;

namespace SuperPong.Components
{
	public class BallComponent : IComponent
	{
		public float width;
		public float height;

		public float direction;
		public float velocity;
	}
}
