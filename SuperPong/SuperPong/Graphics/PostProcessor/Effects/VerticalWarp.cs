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

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SuperPong.Graphics.PostProcessor.Effects
{
    public class VerticalWarp : PostProcessorEffect
    {
        readonly Effect _warpEffect;
        RenderTarget2D _outTarget = null;

        public float Time;
        public float Amplitude;
        public float Speed;
        public float Period = 1.0f;

        public VerticalWarp(PostProcessor postProcessor, ContentManager content) : base(postProcessor)
        {
            _warpEffect = content.Load<Effect>(Constants.Resources.EFFECT_WARP);
        }

        public override void Dispose()
        {

        }

        public override void Process(RenderTarget2D inTarget, out RenderTarget2D outTarget)
        {
            if (_outTarget == null)
            {
                Resize(PostProcessor.Bounds.Width,
                       PostProcessor.Bounds.Height);
            }

            PostProcessor.GraphicsDevice.SetRenderTarget(_outTarget);

            _warpEffect.Parameters["time"].SetValue(Time);
            _warpEffect.Parameters["amplitude"].SetValue(Amplitude);
            _warpEffect.Parameters["speed"].SetValue(Speed);
            _warpEffect.Parameters["period"].SetValue(Period);

            PostProcessor.SpriteBatch.Begin(SpriteSortMode.Deferred,
                                           null,
                                           null,
                                           null,
                                           null,
                                            _warpEffect);
            PostProcessor.SpriteBatch.Draw(inTarget,
                                           _outTarget.Bounds,
                                           Color.White);
            PostProcessor.SpriteBatch.End();

            PostProcessor.GraphicsDevice.SetRenderTarget(null);

            outTarget = _outTarget;
        }

        public override void Resize(int width, int height)
        {
            _outTarget = new RenderTarget2D(PostProcessor.GraphicsDevice,
                                            width,
                                            height,
                                            false,
                                            SurfaceFormat.Color,
                                            DepthFormat.None);
        }

        public override void Update(float dt)
        {

        }
    }
}
