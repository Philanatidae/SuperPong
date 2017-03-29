using ECS;
using Microsoft.Xna.Framework;

namespace SuperPong.Components
{
	public class PaddleComponent : IComponent
	{
		float width;
		float height;
		Vector2 normal;
		bool ignoreCollisions;
	}
}
