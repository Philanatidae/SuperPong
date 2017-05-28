using System;
using ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperPong.Components;

namespace SuperPong.Entities
{
    public static class FieldBackgroundEntity
    {
        public static Entity Create(Engine engine, Texture2D texture)
        {
            Entity entity = engine.CreateEntity();

            entity.AddComponent(new TransformComponent(Vector2.Zero));
            entity.AddComponent(new SpriteComponent(texture, new Vector2(Constants.Pong.FIELD_BACKGROUND_WIDTH,
                                                                         Constants.Pong.FIELD_BACKGROUND_HEIGHT)));
            entity.GetComponent<SpriteComponent>().RenderGroup = Constants.Pong.RENDER_GROUP;

            return entity;
        }
    }
}
