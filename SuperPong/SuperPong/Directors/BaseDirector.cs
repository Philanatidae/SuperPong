using System;
using ECS;
using Microsoft.Xna.Framework.Content;
using SuperPong.Common;
using SuperPong.Graphics.PostProcessor;
using SuperPong.Particles;
using SuperPong.Processes;

namespace SuperPong.Directors
{
    public abstract class BaseDirector
    {
        protected readonly IPongDirectorOwner _owner;
        protected ProcessManager _processManager = new ProcessManager();

        public BaseDirector(IPongDirectorOwner owner)
        {
            _owner = owner;
        }

        public void Update(float dt)
        {
            _processManager.Update(dt);
        }

        public abstract void RegisterEvents();

        public abstract void UnregisterEvents();
    }

    public interface IPongDirectorOwner
    {
        Engine Engine
        {
            get;
        }

        PostProcessor PongPostProcessor
        {
            get;
        }

        ParticleManager<VelocityParticleInfo> VelocityParticleManager
        {
            get;
        }

        PongCamera PongCamera
        {
            get;
        }

        ContentManager Content
        {
            get;
        }

        Player Player1
        {
            get;
        }

        Player Player2
        {
            get;
        }
    }
}
