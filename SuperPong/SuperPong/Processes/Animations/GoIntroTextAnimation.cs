using System;
using ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperPong.Common;
using SuperPong.Components;

namespace SuperPong.Processes.Animations
{
    public class GoIntroTextAnimation : Process
    {
        float _elapsed;
        readonly Engine _engine;
        readonly Entity _entity;
        readonly TransformComponent _transformComp;
        readonly FontComponent _fontComp;
        readonly Camera _camera;
        readonly Viewport _viewport;

        public GoIntroTextAnimation(Engine engine, Entity entity, Camera camera, Viewport viewport)
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
            _viewport = viewport;

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
            _engine.DestroyEntity(_entity);
        }

        protected override void OnTogglePause()
        {

        }

        protected override void OnUpdate(float dt)
        {
            _elapsed += dt;
            if (_elapsed > Constants.Animations.INTRO_GO_DURATION)
            {
                _elapsed = Constants.Animations.INTRO_GO_DURATION;
            }
            UpdateTween();

            if (_elapsed >= Constants.Animations.INTRO_GO_DURATION)
            {
                Kill();
                return;
            }
        }

        void UpdateTween()
        {
            Vector2 bounds = _fontComp.Font.MeasureString(Constants.Pong.INTRO_READY_CONTENT);
            float _width = bounds.X;
            float _height = bounds.Y;

            float startX = _camera.ScreenToWorldCoords(new Vector2(_viewport.Width, 0)).X;
            float endX = _camera.ScreenToWorldCoords(new Vector2(_viewport.Width / 2, 0)).X;

            startX += _width / 2;

            float startY = _camera.ScreenToWorldCoords(new Vector2(0, _viewport.Height / 2)).Y;
            float endY = _camera.ScreenToWorldCoords(new Vector2(0, _viewport.Height)).Y;

            endY += _height / 2;

            float masterAlpha = _elapsed / Constants.Animations.INTRO_READY_DURATION;
            if (masterAlpha < 0.5f)
            {
                float alpha = masterAlpha * 2;
                float beta = Easings.CubicEaseOut(alpha);

                float valX = MathHelper.Lerp(startX, endX, beta);
                _transformComp.SetPosition(valX, 0);
            }
            else
            {
                float alpha = (masterAlpha - 0.5f) * 2;
                float beta = Easings.BackEaseIn(alpha);

                float valY = MathHelper.Lerp(startY, endY, beta);
                _transformComp.SetPosition(0, valY);
            }
        }
    }
}
