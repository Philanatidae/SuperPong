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
using SuperPong.Components;

namespace SuperPong.Systems
{
    public class InputSystem : EntitySystem
    {
        Family _family = Family.All(typeof(PlayerComponent)).Get();
        ImmutableList<Entity> _entities;

        public InputSystem(Engine engine) : base(engine)
        {
            _entities = engine.GetEntitiesFor(_family);
        }

        public bool IsPauseButtonPressed()
        {
            for (int i = 0; i < _entities.Count; i++)
            {
                Entity entity = _entities[i];

                PlayerComponent playerComp = entity.GetComponent<PlayerComponent>();
                if (playerComp.Player.InputMethod.PauseKeyPressed)
                {
                    return true;
                }
            }
            return false;
        }

        public override void Update(float dt)
        {
            foreach (Entity entity in _entities)
            {
                PlayerComponent playerComp = entity.GetComponent<PlayerComponent>();
                playerComp.Player.InputMethod.Update(dt);
            }
        }
    }
}
