using System;
using ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperPong.Common;
using SuperPong.Components;

namespace SuperPong.Processes.Animations
{
    public class GameOverTextAnimation : Process
    {
        float _elapsed;
        readonly Engine _engine;
        readonly Entity _entity;
        readonly TransformComponent _transformComp;
        readonly FontComponent _fontComp;
        readonly Camera _camera;
        readonly GraphicsDevice _graphicsDevice;

        public GameOverTextAnimation(Engine engine, Entity entity, Camera camera, GraphicsDevice graphicsDevice)
        {
            _engine = engine;
            _entity = entity;

            _transformComp = _entity.GetComponent<TransformComponent>();
            if (_transformComp == null)
            {
                throw new Exception("Entity must have TransformComponent");
            }

            _fontComp = _entity.GetComponent<FontComponent>();
            if (_fontComp == null)
            {
                throw new Exception("Entity must have FontComponent");
            }

            _camera = camera;
            _graphicsDevice = graphicsDevice;

            _fontComp.Hidden = true;
        }

        protected override void OnInitialize()
        {
            _elapsed = 0;
            UpdateTween();

            _fontComp.Hidden = false;
        }

        protected override void OnKill()
        {

        }

        protected override void OnTogglePause()
        {

        }

        protected override void OnUpdate(float dt)
        {
            _elapsed += dt;
            if (_elapsed > Constants.Animations.GAME_OVER_DURATION)
            {
                _elapsed = Constants.Animations.GAME_OVER_DURATION;
            }
            UpdateTween();

            if (_elapsed >= Constants.Animations.GAME_OVER_DURATION)
            {
                Kill();
                return;
            }
        }

        void UpdateTween()
        {
            float _width = _fontComp.Font.MeasureString(_fontComp.Content).Width;

            float startX = _camera.ScreenToWorldCoords(new Vector2(_graphicsDevice.Viewport.Width, 0)).X;
            float midX = _camera.ScreenToWorldCoords(new Vector2(_graphicsDevice.Viewport.Width / 2, 0)).X;

            startX += _width / 2;

            float alpha = _elapsed / Constants.Animations.GAME_OVER_DURATION;
            float beta = Easings.CubicEaseOut(alpha);

            float valX = MathHelper.Lerp(startX, midX, beta);
            _transformComp.SetPosition(valX, 0);
        }
    }
}
