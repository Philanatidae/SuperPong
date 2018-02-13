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
