/*
This file is part of Super Pong.

Super Pong is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Super Pong is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Super Pong.  If not, see <http://www.gnu.org/licenses/>.
*/

using ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperPong.Components;

namespace SuperPong.Entities
{
    public static class EdgeEntity
    {
        public static Entity Create(Engine engine, Texture2D texture, Vector2 position, Vector2 normal)
        {
            Entity entity = engine.CreateEntity();

            entity.AddComponent(new TransformComponent(position));
            entity.AddComponent(new SpriteComponent(texture, new Vector2(Constants.Pong.EDGE_WIDTH,
                                                                         Constants.Pong.EDGE_HEIGHT)));
            entity.GetComponent<SpriteComponent>().RenderGroup = Constants.Pong.RENDER_GROUP;
            entity.AddComponent(new EdgeComponent(normal));

            return entity;
        }
    }
}
