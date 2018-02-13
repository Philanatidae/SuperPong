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
using Microsoft.Xna.Framework.Content;

namespace SuperPong.Content
{
    public class LockingContentManager : ContentManager
    {
        public bool Locked
        {
            get;
            set;
        }

        public LockingContentManager(IServiceProvider services) : base(services)
        {
        }

        public override T Load<T>(string assetName)
        {
            if (Locked && !LoadedAssets.ContainsKey(assetName))
            {
                throw new ContentLockedException();
            }

            return base.Load<T>(assetName);
        }

    }
}
