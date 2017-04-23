using ECS;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;
using SuperPong.Components;

namespace SuperPong.Entities
{
    public static class LivesEntity
    {
        public static Entity Create(Engine engine, BitmapFont font, Vector2 position, Player forPlayer, int lives)
        {
            Entity entity = engine.CreateEntity();

            entity.AddComponent(new TransformComponent(position));
            entity.AddComponent(new FontComponent(font, lives.ToString()));
            entity.AddComponent(new LivesComponent(forPlayer, lives));

            return entity;
        }
    }
}
