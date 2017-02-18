using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Linq
{
    public static class Enumerable
    {
        static Exception MoreThanOneElement()
        {
            return (Exception)new InvalidOperationException("more than one element");
        }

        public static bool Any<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null) { throw new ArgumentNullException("source"); }

            using (IEnumerator<TSource> enumerator = source.GetEnumerator())
            {
                if (enumerator.MoveNext())
                    return true;
            }
            return false;
        }

        public static TSource SingleOrDefault<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null) { throw new ArgumentNullException("source"); }

            IList<TSource> list = source as IList<TSource>;
            if (list != null)
            {
                switch (list.Count)
                {
                    case 0:
                        return default(TSource);
                    case 1:
                        return list[0];
                }
            }
            else
            {
                using (IEnumerator<TSource> enumerator = source.GetEnumerator())
                {
                    if (!enumerator.MoveNext())
                        return default(TSource);
                    TSource current = enumerator.Current;
                    if (!enumerator.MoveNext())
                        return current;
                }
            }
            throw MoreThanOneElement();
        }

        

        public static TSource LastOrDefault<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null) { throw new ArgumentNullException("source"); }
            IList<TSource> list = source as IList<TSource>;
            if (list != null)
            {
                int count = list.Count;
                if (count > 0)
                    return list[count - 1];
            }
            else
            {
                using (IEnumerator<TSource> enumerator = source.GetEnumerator())
                {
                    if (enumerator.MoveNext())
                    {
                        TSource current;
                        do
                        {
                            current = enumerator.Current;
                        }
                        while (enumerator.MoveNext());
                        return current;
                    }
                }
            }
            return default(TSource);
        }

        public static int Max(this IEnumerable<int> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            int num1 = 0;
            bool flag = false;
            foreach (int num2 in source)
            {
                if (flag)
                {
                    if (num2 > num1)
                        num1 = num2;
                }
                else
                {
                    num1 = num2;
                    flag = true;
                }
            }
            if (flag)
                return num1;
            throw new InvalidOperationException("No Elements");
        }

        

        public static long Max(this IEnumerable<long> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            long num1 = 0L;
            bool flag = false;
            foreach (long num2 in source)
            {
                if (flag)
                {
                    if (num2 > num1)
                        num1 = num2;
                }
                else
                {
                    num1 = num2;
                    flag = true;
                }
            }
            if (flag)
                return num1;
            throw new InvalidOperationException("No Elements");
        }

        //public static int? Max(this IEnumerable<int?> source)
        //{
        //    if (source == null)
        //        throw new ArgumentNullException("source");
        //    int? nullable1 = new int?();
        //    foreach (int? nullable2 in source)
        //    {
        //        if (nullable1.HasValue)
        //        {
        //            int? nullable3 = nullable2;
        //            int? nullable4 = nullable1;
        //            if ((nullable3.GetValueOrDefault() <= nullable4.GetValueOrDefault() ? 0 : (nullable3.HasValue & nullable4.HasValue ? 1 : 0)) == 0)
        //                continue;
        //        }
        //        nullable1 = nullable2;
        //    }
        //    return nullable1;
        //}

        //public static long? Max(this IEnumerable<long?> source)
        //{
        //    if (source == null)
        //        throw new ArgumentNullException("source");
        //    long? nullable1 = new long?();
        //    foreach (long? nullable2 in source)
        //    {
        //        if (nullable1.HasValue)
        //        {
        //            long? nullable3 = nullable2;
        //            long? nullable4 = nullable1;
        //            if ((nullable3.GetValueOrDefault() <= nullable4.GetValueOrDefault() ? 0 : (nullable3.HasValue & nullable4.HasValue ? 1 : 0)) == 0)
        //                continue;
        //        }
        //        nullable1 = nullable2;
        //    }
        //    return nullable1;
        //}

        //public static double Max(this IEnumerable<double> source)
        //{
        //    if (source == null)
        //        throw new ArgumentNullException("source");
        //    double d = 0.0;
        //    bool flag = false;
        //    foreach (double num in source)
        //    {
        //        if (flag)
        //        {
        //            if (num > d || double.IsNaN(d))
        //                d = num;
        //        }
        //        else
        //        {
        //            d = num;
        //            flag = true;
        //        }
        //    }
        //    if (flag)
        //        return d;
        //    throw new InvalidOperationException("No Elements");
        //}

        //public static double? Max(this IEnumerable<double?> source)
        //{
        //    if (source == null)
        //        throw new ArgumentNullException("source");
        //    double? nullable1 = new double?();
        //    foreach (double? nullable2 in source)
        //    {
        //        if (nullable2.HasValue)
        //        {
        //            if (nullable1.HasValue)
        //            {
        //                double? nullable3 = nullable2;
        //                double? nullable4 = nullable1;
        //                if ((nullable3.GetValueOrDefault() <= nullable4.GetValueOrDefault() ? 0 : (nullable3.HasValue & nullable4.HasValue ? 1 : 0)) == 0 && !double.IsNaN(nullable1.Value))
        //                    continue;
        //            }
        //            nullable1 = nullable2;
        //        }
        //    }
        //    return nullable1;
        //}

        //public static float Max(this IEnumerable<float> source)
        //{
        //    if (source == null)
        //        throw new ArgumentNullException("source");
        //    float num1 = 0.0f;
        //    bool flag = false;
        //    foreach (float num2 in source)
        //    {
        //        if (flag)
        //        {
        //            if ((double)num2 > (double)num1 || double.IsNaN((double)num1))
        //                num1 = num2;
        //        }
        //        else
        //        {
        //            num1 = num2;
        //            flag = true;
        //        }
        //    }
        //    if (flag)
        //        return num1;
        //    throw new InvalidOperationException("No Elements");
        //}

        //public static float? Max(this IEnumerable<float?> source)
        //{
        //    if (source == null)
        //        throw new ArgumentNullException("source");
        //    float? nullable1 = new float?();
        //    foreach (float? nullable2 in source)
        //    {
        //        if (nullable2.HasValue)
        //        {
        //            if (nullable1.HasValue)
        //            {
        //                float? nullable3 = nullable2;
        //                float? nullable4 = nullable1;
        //                if (((double)nullable3.GetValueOrDefault() <= (double)nullable4.GetValueOrDefault() ? 0 : (nullable3.HasValue & nullable4.HasValue ? 1 : 0)) == 0 && !float.IsNaN(nullable1.Value))
        //                    continue;
        //            }
        //            nullable1 = nullable2;
        //        }
        //    }
        //    return nullable1;
        //}

        //public static Decimal Max(this IEnumerable<Decimal> source)
        //{
        //    if (source == null)
        //        throw new ArgumentNullException("source");
        //    Decimal num1 = new Decimal(0);
        //    bool flag = false;
        //    foreach (Decimal num2 in source)
        //    {
        //        if (flag)
        //        {
        //            if (num2 > num1)
        //                num1 = num2;
        //        }
        //        else
        //        {
        //            num1 = num2;
        //            flag = true;
        //        }
        //    }
        //    if (flag)
        //        return num1;
        //    throw new InvalidOperationException("No Elements");
        //}

        //public static Decimal? Max(this IEnumerable<Decimal?> source)
        //{
        //    if (source == null)
        //        throw new ArgumentNullException("source");
        //    Decimal? nullable1 = new Decimal?();
        //    foreach (Decimal? nullable2 in source)
        //    {
        //        if (nullable1.HasValue)
        //        {
        //            Decimal? nullable3 = nullable2;
        //            Decimal? nullable4 = nullable1;
        //            if ((!(nullable3.GetValueOrDefault() > nullable4.GetValueOrDefault()) ? 0 : (nullable3.HasValue & nullable4.HasValue ? 1 : 0)) == 0)
        //                continue;
        //        }
        //        nullable1 = nullable2;
        //    }
        //    return nullable1;
        //}

        //public static TSource Max<TSource>(this IEnumerable<TSource> source)
        //{
        //    if (source == null)
        //        throw new ArgumentNullException("source");
        //    Comparer<TSource> @default = Comparer<TSource>.Default;
        //    TSource y = default(TSource);
        //    if ((object)y == null)
        //    {
        //        foreach (TSource x in source)
        //        {
        //            if ((object)x != null && ((object)y == null || @default.Compare(x, y) > 0))
        //                y = x;
        //        }
        //        return y;
        //    }
        //    bool flag = false;
        //    foreach (TSource x in source)
        //    {
        //        if (flag)
        //        {
        //            if (@default.Compare(x, y) > 0)
        //                y = x;
        //        }
        //        else
        //        {
        //            y = x;
        //            flag = true;
        //        }
        //    }
        //    if (flag)
        //        return y;
        //    throw new InvalidOperationException("No Elements");
        //}

        public static int Max<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
        {
            return Enumerable.Max(source.Select<TSource, int>(selector));
        }

        public static long Max<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector)
        {
            return Enumerable.Max(source.Select<TSource, long>(selector));
        }

        //public static int? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector)
        //{
        //    return Enumerable.Max(source.Select<TSource, int?>(selector));
        //}


        //public static long? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector)
        //{
        //    return Enumerable.Max(source.Select<TSource, long?>(selector));
        //}

        //public static float Max<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
        //{
        //    return Enumerable.Max(source.Select<TSource, float>(selector));
        //}

        //public static float? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector)
        //{
        //    return Enumerable.Max(source.Select<TSource, float?>(selector));
        //}

        //public static double Max<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
        //{
        //    return Enumerable.Max(source.Select<TSource, double>(selector));
        //}

        //public static double? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector)
        //{
        //    return Enumerable.Max(source.Select<TSource, double?>(selector));
        //}

        //public static Decimal Max<TSource>(this IEnumerable<TSource> source, Func<TSource, Decimal> selector)
        //{
        //    return Enumerable.Max(source.Select<TSource, Decimal>(selector));
        //}

        //public static Decimal? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, Decimal?> selector)
        //{
        //    return Enumerable.Max(source.Select<TSource, Decimal?>(selector));
        //}

        //public static TResult Max<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        //{
        //    return Enumerable.Max<TResult>(source.Select<TSource, TResult>(selector));
        //}
    }
}
