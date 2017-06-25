using ECS;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;
using SuperPong.Components;

namespace SuperPong.Entities
{
    public static class FontEntity
    {
        public static Entity Create(Engine engine, Vector2 position, BitmapFont font, string content)
        {
            Entity entity = engine.CreateEntity();

            TransformComponent transformComp = new TransformComponent(position);
            entity.AddComponent(transformComp);

            FontComponent fontComp = new FontComponent(font, content);
            entity.AddComponent(fontComp);

            return entity;
        }
    }
}
