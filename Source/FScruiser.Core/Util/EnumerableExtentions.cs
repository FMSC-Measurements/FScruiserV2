using System.Collections.Generic;
namespace System.Linq
{
    public static class EnumerableExtentions
    {
        public static IEnumerable<TSource> OrEmpty<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null) return Enumerable.Empty<TSource>();
            else return source;
        }

        public static IEnumerable<TSource> Prepend<TSource>(this IEnumerable<TSource> source, TSource item)
        {
            if(source == null) { throw new ArgumentNullException("source"); }

            yield return item;
            foreach(var i in source)
            {
                yield return i;
            }
        }
    }
}