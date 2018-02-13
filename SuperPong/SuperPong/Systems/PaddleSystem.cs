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

using System;
using ECS;
using Microsoft.Xna.Framework;
using SuperPong.Components;

namespace SuperPong.Systems
{
    public class PaddleSystem : EntitySystem
    {
        Family _family = Family.All(typeof(PaddleComponent), typeof(PlayerComponent), typeof(TransformComponent)).Get();
        ImmutableList<Entity> _entities;

        public PaddleSystem(Engine engine) : base(engine)
        {
            _entities = engine.GetEntitiesFor(_family);
        }

        public override void Update(float dt)
        {
            foreach (Entity entity in _entities)
            {
                PaddleComponent paddleComp = entity.GetComponent<PaddleComponent>();
                PlayerComponent playerComp = entity.GetComponent<PlayerComponent>();
                TransformComponent transformComp = entity.GetComponent<TransformComponent>();

                Vector2 delta = Vector2.Zero;

                delta.Y += Constants.Pong.PADDLE_SPEED * playerComp.Input.GetAxis() * dt;

                transformComp.Move(delta);

                if (transformComp.Position.Y + paddleComp.Bounds.Y / 2 > Constants.Pong.PLAYFIELD_HEIGHT / 2)
                {
                    transformComp.SetPosition(transformComp.Position.X,
                                              Constants.Pong.PLAYFIELD_HEIGHT / 2 - paddleComp.Bounds.Y / 2);
                }
                if (transformComp.Position.Y - paddleComp.Bounds.Y / 2 < -Constants.Pong.PLAYFIELD_HEIGHT / 2)
                {
                    transformComp.SetPosition(transformComp.Position.X,
                                              -Constants.Pong.PLAYFIELD_HEIGHT / 2 + paddleComp.Bounds.Y / 2);
                }
            }
        }
    }
}
