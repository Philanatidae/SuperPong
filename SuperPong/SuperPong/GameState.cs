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
using SuperPong.Content;

namespace SuperPong
{
    public abstract class GameState : IDisposable
    {
        protected GameManager GameManager
        {
            get;
            private set;
        }
        public LockingContentManager Content
        {
            get;
            private set;
        }

        public GameState(GameManager gameManager)
        {
            GameManager = gameManager;

            Content = new LockingContentManager(gameManager.Services);
            Content.RootDirectory = "Content";
        }

        public abstract void Initialize();

        public abstract void LoadContent();

        public void UnloadContent()
        {
            Content.Unload();
        }

        public abstract void Show();

        public abstract void Hide();

        public abstract void Update(float dt);

        public abstract void Draw(float dt);

        public abstract void Dispose();
    }
}
