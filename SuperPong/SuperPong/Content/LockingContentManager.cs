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
