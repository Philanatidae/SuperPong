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
using Events;
using SuperPong.Components;
using SuperPong.Events;

namespace SuperPong.Systems
{
    public class LivesSystem : EntitySystem, IEventListener
    {
        readonly Family _livesFamily = Family.All(typeof(LivesComponent), typeof(FontComponent)).Get();

        readonly ImmutableList<Entity> _livesEntities;

        public LivesSystem(Engine engine) : base(engine)
        {
            _livesEntities = engine.GetEntitiesFor(_livesFamily);
        }

        public override void Update(float dt)
        {
            foreach (Entity entity in _livesEntities)
            {
                LivesComponent livesComp = entity.GetComponent<LivesComponent>();
                FontComponent fontComp = entity.GetComponent<FontComponent>();

                fontComp.Content = livesComp.Lives.ToString();
            }
        }

        void DecreaseLifeCount(Player player)
        {
            foreach (Entity livesEntity in _livesEntities)
            {
                LivesComponent livesComp = livesEntity.GetComponent<LivesComponent>();

                if (livesComp.For == player)
                {
                    livesComp.Lives--;
                    break;
                }
            }
        }

        public void RegisterEventListeners()
        {
            EventManager.Instance.RegisterListener<GoalEvent>(this);
        }

        public void UnregisterEventListeners()
        {
            EventManager.Instance.UnregisterListener(this);
        }

        public bool Handle(IEvent evt)
        {
            if (evt is GoalEvent)
            {
                GoalEvent goalEvent = evt as GoalEvent;
                DecreaseLifeCount(goalEvent.Goal.GetComponent<GoalComponent>().For);
            }

            return false;
        }
    }
}
