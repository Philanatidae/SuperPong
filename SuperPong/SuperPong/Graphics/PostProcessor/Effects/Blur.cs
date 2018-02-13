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

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace SuperPong.Graphics.PostProcessor.Effects
{
    class Blur : PostProcessorEffect
    {
        readonly Effect _blurEffect;
        RenderTarget2D _hBlurTarget = null;
        RenderTarget2D _vBlurTarget = null;

        public float Radius = 1.0f;

        public Blur(PostProcessor postProcessor, ContentManager content) : base(postProcessor)
        {
            _blurEffect = content.Load<Effect>(Constants.Resources.EFFECT_BLUR);
        }

        public override void Dispose()
        {

        }

        public override void Process(RenderTarget2D inTarget, out RenderTarget2D outTarget)
        {
            if(_hBlurTarget == null || _vBlurTarget == null)
            {
                Resize(PostProcessor.Bounds.Width,
                    PostProcessor.Bounds.Height);
            }

            // Global uniforms
            _blurEffect.Parameters["radius"].SetValue(Radius);

            // Horizontal blur
            PostProcessor.GraphicsDevice.SetRenderTarget(_hBlurTarget);

            _blurEffect.Parameters["direction"].SetValue(Vector2.UnitX);
            _blurEffect.Parameters["resolution"].SetValue((float) _hBlurTarget.Width);

            PostProcessor.SpriteBatch.Begin(SpriteSortMode.Deferred,
                null,
                null,
                null,
                null,
                _blurEffect);
            PostProcessor.SpriteBatch.Draw(inTarget,
                _hBlurTarget.Bounds,
                Color.White);
            PostProcessor.SpriteBatch.End();

            // Vertical blur
            _blurEffect.Parameters["direction"].SetValue(Vector2.UnitY);
            _blurEffect.Parameters["resolution"].SetValue((float)_vBlurTarget.Height);

            PostProcessor.GraphicsDevice.SetRenderTarget(_vBlurTarget);
            PostProcessor.SpriteBatch.Begin(SpriteSortMode.Deferred,
                null,
                null,
                null,
                null,
                _blurEffect);
            PostProcessor.SpriteBatch.Draw(_hBlurTarget,
                _vBlurTarget.Bounds,
                Color.White);
            PostProcessor.SpriteBatch.End();
            PostProcessor.GraphicsDevice.SetRenderTarget(null);

            outTarget = _vBlurTarget;
        }

        public override void Resize(int width, int height)
        {
            _hBlurTarget = new RenderTarget2D(PostProcessor.GraphicsDevice,
                width,
                height,
                false,
                SurfaceFormat.Color,
                DepthFormat.None);
            _vBlurTarget = new RenderTarget2D(PostProcessor.GraphicsDevice,
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
