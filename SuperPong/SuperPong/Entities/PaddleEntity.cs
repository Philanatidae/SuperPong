using ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperPong.Components;

namespace SuperPong.Entities
{
    public static class PaddleEntity
    {
        public static Entity Create(Engine engine, Texture2D texture, Vector2 position, Vector2 normal)
        {
            Entity entity = engine.CreateEntity();

            entity.AddComponent(new TransformComponent(position));
            entity.AddComponent(new PaddleComponent(normal));

            entity.AddComponent(new SpriteComponent(texture, entity.GetComponent<PaddleComponent>().Bounds));
            entity.GetComponent<SpriteComponent>().RenderGroup = Constants.Pong.RENDER_GROUP;

            return entity;
        }
    }
}
