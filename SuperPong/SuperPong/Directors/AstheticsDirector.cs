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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperPong.Common;
using SuperPong.Components;
using SuperPong.Events;
using SuperPong.Particles;

namespace SuperPong.Directors
{
    public class AstheticsDirector : BaseDirector, IEventListener
    {
        readonly MTRandom _random = new MTRandom();

        readonly Family _edgeFamily = Family.All(typeof(EdgeComponent), typeof(TransformComponent)).Get();
        readonly ImmutableList<Entity> _edgeEntities;

        public AstheticsDirector(IPongDirectorOwner owner) : base(owner)
        {
            _edgeEntities = _owner.Engine.GetEntitiesFor(_edgeFamily);
        }

        public override void RegisterEvents()
        {
            EventManager.Instance.RegisterListener<GoalEvent>(this);
        }

        public override void UnregisterEvents()
        {
            EventManager.Instance.UnregisterListener(this);
        }

        public bool Handle(IEvent evt)
        {
            if (evt is GoalEvent)
            {
                HandleGoal(evt as GoalEvent);
            }

            return false;
        }

        void CreateExplosion(Vector2 position)
        {
            for (int i = 0; i < 150; i++)
            {
                float speed = 2000 * (1f - 1 / _random.NextSingle(1, 10));
                float dir = _random.NextSingle(0, MathHelper.TwoPi);
                VelocityParticleInfo info = new VelocityParticleInfo()
                {
                    Velocity = new Vector2((float)(speed * Math.Cos(dir)), (float)(speed * Math.Sin(dir))),
                    LengthMultiplier = 1f,
                    EdgeEntities = _edgeEntities
                };

                _owner.VelocityParticleManager.CreateParticle(_owner.Content.Load<Texture2D>(Constants.Resources.TEXTURE_PARTICLE_VELOCITY),
                                                        position,
                                                        Color.White,
                                                        150,
                                                        Vector2.One,
                                                        info);
            }
        }

        // HANDLERS!
        void HandleGoal(GoalEvent goalEvent)
        {
            // Create an explosion
            CreateExplosion(goalEvent.Position);
        }
    }
}
