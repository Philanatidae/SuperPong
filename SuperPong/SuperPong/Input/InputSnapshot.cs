using System;

namespace SuperPong.Input
{
    public class InputSnapshot
    {
        internal float _axis;

        public InputSnapshot() : this(0)
        {
        }

        public InputSnapshot(float axis)
        {
            _axis = axis;
        }

        public float GetAxis()
        {
            return _axis;
        }
    }
}
