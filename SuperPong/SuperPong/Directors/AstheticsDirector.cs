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
