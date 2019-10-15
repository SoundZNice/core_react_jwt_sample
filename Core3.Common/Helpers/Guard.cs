using System;
using System.Collections.Generic;
using System.Linq;

namespace Core3.Common.Helpers
{
    public static class Guard
    {
        public static void ArgumentNotNull(object arg, string argName)
        {
            if (arg == null)
                throw new ArgumentNullException(argName);
        }

        public static void ArgumentGuidNotEmpty(Guid arg, string argName)
        {
            if (arg == Guid.Empty)
                throw new ArgumentException(argName);
        }

        public static void ArgumentCollectionNotNull<T>(IEnumerable<T> arg, string argName)
        {
            if (arg == null || !arg.Any())
                throw new ArgumentException(argName);
        }

        public static void ArgumentStringNotNull(string arg, string argName)
        {
            if (string.IsNullOrWhiteSpace(arg))
                throw new ArgumentException(argName);
        }
    }
}
