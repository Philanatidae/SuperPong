namespace SuperPong.Common
{
    public static class MathUtils
    {
        public static float Clamp(float min, float max, float val)
        {
            if (val < min)
            {
                return min;
            }
            if (val > max)
            {
                return max;
            }
            return val;
        }
    }
}
