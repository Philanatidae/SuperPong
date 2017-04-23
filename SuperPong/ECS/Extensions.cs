using System;
namespace ECS
{
    public static class Extensions
    {
        public static bool IsComponent(this Type type)
        {
            return typeof(IComponent).IsAssignableFrom(type);
        }

        public static bool IsEquivalent(this object[] a, object[] b)
        {
            if (a.Length != b.Length)
            {
                return false;
            }

            foreach (object objA in a)
            {
                bool found = false;
                foreach (object objB in b)
                {
                    if (objA.Equals(objB))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
