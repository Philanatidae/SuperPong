using ECS;
using Microsoft.Xna.Framework;

namespace SuperPong.Components
{
	public class GoalComponent : IComponent
	{
		public GoalComponent()
		{
		}

		public GoalComponent(Vector2 normal)
		{
			Normal = normal;
		}

		public Vector2 Normal;
	}
}
