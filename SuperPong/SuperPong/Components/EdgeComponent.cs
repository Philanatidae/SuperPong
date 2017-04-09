using ECS;
using Microsoft.Xna.Framework;

namespace SuperPong.Components
{
	public class EdgeComponent :IComponent
	{
		public EdgeComponent()
		{
		}

		public EdgeComponent(Vector2 normal)
		{
			Normal = normal;
		}

		public Vector2 Normal;
	}
}
