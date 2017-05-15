﻿using System;
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

        public PostProcessor(GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            Bounds = new Rectangle(0, 0,
                                   graphicsDevice.Viewport.Width,
                                   graphicsDevice.Viewport.Height);
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

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < Effects.Count; i++)
            {
                Effects[i].Update(gameTime);
            }
        }

        public void End()
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
            SpriteBatch.Begin();
            SpriteBatch.Draw(finalTarget, Bounds, Color.White);
            SpriteBatch.End();
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
