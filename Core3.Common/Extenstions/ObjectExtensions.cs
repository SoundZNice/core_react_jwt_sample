using System;

namespace Core3.Common.Extenstions
{
    public static class ObjectExtensions
    {
        public static T Do<T>(this T obj, Action<T> action)
        {
            if (obj != null)
                action?.Invoke(obj);

            return obj;
        }
    }
}
