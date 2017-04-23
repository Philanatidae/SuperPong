using System;
using Microsoft.Xna.Framework;

namespace SuperPong.Processes
{
    public abstract class Command : Process
    {
        protected override void OnInitialize()
        {
            OnTrigger();
            Kill();
        }

        protected abstract void OnTrigger();

        protected override void OnKill()
        {
        }

        protected override void OnTogglePause()
        {
        }

        protected override void OnUpdate(GameTime gameTime)
        {
        }
    }
}
