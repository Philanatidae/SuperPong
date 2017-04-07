using ECS;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;

namespace SuperPong.Components
{
	public class FontComponent : IComponent
	{
		public FontComponent()
		{
		}

		public FontComponent(BitmapFont font, string content)
		{
			Font = font;
			Content = content;
		}

		public BitmapFont Font;
		public string Content;
		public Color Color = Color.White;
	}
}
