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

        public override void Update(GameTime gameTime)
        {

        }
    }
}
