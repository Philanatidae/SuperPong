using System;

namespace SuperPong.Content
{
    public class ContentLockedException : Exception
    {
        public ContentLockedException() : base("ContentManager is locked.")
        {
        }
    }
}
