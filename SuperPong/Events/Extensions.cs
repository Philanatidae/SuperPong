using System;
namespace Events
{
    public static class Extensions
    {
        public static bool IsEvent(this Type type)
        {
            return typeof(IEvent).IsAssignableFrom(type);
        }
    }
}
