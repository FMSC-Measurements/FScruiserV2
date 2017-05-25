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
    }
}