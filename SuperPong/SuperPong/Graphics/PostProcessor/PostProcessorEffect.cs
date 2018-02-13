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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperPong.Graphics.PostProcessor
{
    public abstract class PostProcessorEffect : IDisposable
    {
        internal readonly PostProcessor PostProcessor;

        public PostProcessorEffect(PostProcessor postProcessor)
        {
            PostProcessor = postProcessor;
        }

        public abstract void Resize(int width, int height);

        public abstract void Update(float dt);

        public abstract void Process(RenderTarget2D inTarget, out RenderTarget2D outTarget);

        public abstract void Dispose();
    }
}
