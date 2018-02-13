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
using System.Collections.Generic;
using Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperPong.Events;
using SuperPong.Exceptions;

namespace SuperPong.Graphics.PostProcessor
{
    public class PostProcessor : IEventListener, IDisposable
    {
        public readonly GraphicsDevice GraphicsDevice;
        internal SpriteBatch SpriteBatch;

        public bool Drawing
        {
            get;
            private set;
        }

        public Rectangle Bounds
        {
            get
            {
                return _renderTarget.Bounds;
            }
            private set
            {
                _renderTarget = new RenderTarget2D(GraphicsDevice,
                                             value.Width,
                                             value.Height,
                                             false,
                                             SurfaceFormat.Color,
                                             DepthFormat.None);

                for (int i = 0; i < Effects.Count; i++)
                {
                    Effects[i].Resize(value.Width, value.Height);
                }
            }
        }

        RenderTarget2D _renderTarget;

        public List<PostProcessorEffect> Effects = new List<PostProcessorEffect>();

        public PostProcessor(GraphicsDevice graphicsDevice, float bufferWidth, float bufferHeight)
        {
            GraphicsDevice = graphicsDevice;
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            Bounds = new Rectangle(0, 0,
                                (int)bufferWidth,
                                (int)bufferHeight);
        }

        public void Begin()
        {
            if (Drawing)
            {
                throw new AlreadyDrawingException();
            }
            Drawing = true;

            GraphicsDevice.SetRenderTarget(_renderTarget);
        }

        public void Update(float dt)
        {
            for (int i = 0; i < Effects.Count; i++)
            {
                Effects[i].Update(dt);
            }
        }

        public RenderTarget2D End()
        {
            return End(true);
        }

        public RenderTarget2D End(bool draw)
        {
            if (!Drawing)
            {
                throw new NotDrawingException();
            }
            Drawing = false;

            GraphicsDevice.SetRenderTarget(null);

            RenderTarget2D finalTarget = _renderTarget;
            // Post-process
            for (int i = 0; i < Effects.Count; i++)
            {
                Effects[i].Process(finalTarget, out finalTarget);
            }

            // Render as a fullscreen quad
            if (draw)
            {
                SpriteBatch.Begin();
                SpriteBatch.Draw(finalTarget, Bounds, Color.White);
                SpriteBatch.End();
            }
            return finalTarget;
        }

        public bool Handle(IEvent evt)
        {
            ResizeEvent resizeEvent = evt as ResizeEvent;

            if (resizeEvent != null)
            {
                Bounds = new Rectangle(0, 0,
                                       resizeEvent.Width,
                                       resizeEvent.Height);
            }

            return false;
        }

        public void Dispose()
        {
            SpriteBatch.Dispose();
            _renderTarget.Dispose();

            for (int i = 0; i < Effects.Count; i++)
            {
                Effects[i].Dispose();
            }
        }
    }
}
