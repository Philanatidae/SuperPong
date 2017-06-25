using System;
using ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperPong.Common;
using SuperPong.Components;

namespace SuperPong.Processes.Animations
{
    public class ReadyIntroTextAnimation : Process
    {
        float _elapsed;
        readonly Engine _engine;
        readonly Entity _entity;
        readonly TransformComponent _transformComp;
        readonly FontComponent _fontComp;
        readonly Camera _camera;
        readonly Viewport _viewport;

        public ReadyIntroTextAnimation(Engine engine, Entity entity, Camera camera, Viewport viewport)
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
            if (_elapsed > Constants.Animations.INTRO_READY_DURATION)
            {
                _elapsed = Constants.Animations.INTRO_READY_DURATION;
            }
            UpdateTween();

            if (_elapsed >= Constants.Animations.INTRO_READY_DURATION)
            {
                Kill();
                return;
            }
        }

        void UpdateTween()
        {
            float _width = _fontComp.Font.MeasureString(Constants.Pong.INTRO_READY_CONTENT).Width;

            float startX = _camera.ScreenToWorldCoords(new Vector2(_viewport.Width, 0)).X;
            float midX = _camera.ScreenToWorldCoords(new Vector2(_viewport.Width / 2, 0)).X;
            float endX = _camera.ScreenToWorldCoords(new Vector2(0, 0)).X;

            startX += _width / 2;
            endX -= _width / 2;

            float masterAlpha = _elapsed / Constants.Animations.INTRO_READY_DURATION;
            if (masterAlpha < 0.5f)
            {
                float alpha = masterAlpha * 2;
                float beta = Easings.CubicEaseOut(alpha);

                float valX = MathHelper.Lerp(startX, midX, beta);
                _transformComp.SetPosition(valX, 0);
            }
            else
            {
                float alpha = (masterAlpha - 0.5f) * 2;
                float beta = Easings.CubicEaseIn(alpha);

                float valX = MathHelper.Lerp(midX, endX, beta);
                _transformComp.SetPosition(valX, 0);
            }
        }
    }
}
