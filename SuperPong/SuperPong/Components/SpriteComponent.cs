using ECS;
using Microsoft.Xna.Framework.Graphics;

namespace SuperPong.Components
{
	public class SpriteComponent : IComponent
	{
		Texture texture;
		float width;
		float height;
	}
}
