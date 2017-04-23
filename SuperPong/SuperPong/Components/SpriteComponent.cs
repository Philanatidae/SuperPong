using ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperPong.Components
{
    public class SpriteComponent : IComponent
    {
        public SpriteComponent()
        {
        }

        public SpriteComponent(Texture2D texture, Vector2 bounds)
        {
            Texture = texture;
            Bounds = bounds;
        }

        public Texture2D Texture;
        public Vector2 Bounds;

        public byte RenderGroup = 0x1;
    }
}
