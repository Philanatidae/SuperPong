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
