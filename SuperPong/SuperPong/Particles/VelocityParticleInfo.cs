using System;
using ECS;
using Microsoft.Xna.Framework;
using SuperPong.Components;

namespace SuperPong.Particles
{
    public struct VelocityParticleInfo
    {
        public Vector2 Velocity;
        public float LengthMultiplier;
        public ImmutableList<Entity> EdgeEntities;

        public static void UpdateParticle(ParticleManager<VelocityParticleInfo>.Particle particle, float dt)
        {
            Vector2 velocity = particle.UserInfo.Velocity;

            particle.Position += velocity * dt;
            particle.Rotation = (float)Math.Atan2(velocity.Y, velocity.X);

            for (int i = 0; i < particle.UserInfo.EdgeEntities.Count; i++)
            {
                Entity entity = particle.UserInfo.EdgeEntities[i];
                TransformComponent transformComp = entity.GetComponent<TransformComponent>();
                EdgeComponent edgeComp = entity.GetComponent<EdgeComponent>();

                // If top edge
                if (edgeComp.Normal.Y < 0
                    && particle.Position.Y >= transformComp.Position.Y)
                {
                    velocity.Y = -Math.Abs(velocity.Y);
                }
                else if (edgeComp.Normal.Y > 0
                  && particle.Position.Y <= transformComp.Position.Y)
                {
                    velocity.Y = Math.Abs(velocity.Y);
                }
            }

            float speed = velocity.Length();
            float alpha = Math.Min(1, Math.Min(particle.PercentLife * 2, speed * dt));
            alpha *= alpha;

            particle.Color.A = (byte)(255 * alpha);

            particle.Scale.X = particle.UserInfo.LengthMultiplier * Math.Min(Math.Min(1, 0.2f * speed * dt + 0.1f), alpha);

            // If within tolerance, set the velocity to zero.
            // We don't have to use sqrt here, so we shouldn't
            if (Math.Abs(velocity.X) + Math.Abs(velocity.Y) < 0.00000000001f)
            {
                velocity = Vector2.Zero;
            }

            float speedMul = 0.9f; // Particles gradually slow down
            velocity *= speedMul;

            particle.UserInfo.Velocity = velocity;
        }
    }
}
