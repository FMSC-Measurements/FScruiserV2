using System;

using System.Collections.Generic;
using System.Text;

namespace System.Linq
{
    public static class IEnumerableExtensions
    {
        public static List<T> ToList<T>(this IEnumerable<T> source)
        {
            if (source == null) { throw new ArgumentNullException("source"); }
            return new List<T>(source);
        }

        public static T[] ToArray<T>(this IEnumerable<T> source)
        {
            if (source == null) { throw new ArgumentNullException("source"); }
            var list = new List<T>(source);
            return list.ToArray();
        }

        public static bool Any<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (predicate == null) throw new ArgumentNullException("predicate");
            foreach (T element in source)
            {
                if (predicate(element)) return true;
            }
            return false;
        }
    }
}
