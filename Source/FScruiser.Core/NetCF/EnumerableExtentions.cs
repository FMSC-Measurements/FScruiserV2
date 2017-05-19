using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Linq
{
    public static class Enumerable
    {
        static Exception MoreThanOneElement()
        {
            return new InvalidOperationException("more than one element");
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

        public static IEnumerable<TSource> Distinct<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null) { throw new ArgumentNullException("source"); }
            return Enumerable.DistinctIterator<TSource>(source, (IEqualityComparer<TSource>)null);
        }

        public static IEnumerable<TSource> Distinct<TSource>(this IEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
        {
            if (source == null) { throw new ArgumentNullException("source"); }
            return Enumerable.DistinctIterator<TSource>(source, comparer);
        }

        private static IEnumerable<TSource> DistinctIterator<TSource>(IEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
        {
            Set<TSource> set = new Set<TSource>(comparer);
            foreach (TSource source1 in source)
            {
                if (set.Add(source1))
                    yield return source1;
            }
        }

        public static TSource First<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null) { throw new ArgumentNullException("source"); }

            IList<TSource> list = source as IList<TSource>;
            if (list != null)
            {
                if (list.Count > 0)
                    return list[0];
            }
            else
            {
                using (IEnumerator<TSource> enumerator = source.GetEnumerator())
                {
                    if (enumerator.MoveNext())
                        return enumerator.Current;
                }
            }
            throw new InvalidOperationException("No Elements");
        }

        /// <summary>
        /// Projects each element of a sequence to an <see cref="IEnumerable{T}" /> 
        /// and flattens the resulting sequences into one sequence.
        /// </summary>

        public static IEnumerable<TResult> SelectMany<TSource, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, IEnumerable<TResult>> selector)
        {
            if (selector == null) throw new ArgumentNullException("selector");

            return source.SelectMany((item, i) => selector(item));
        }

        /// <summary>
        /// Projects each element of a sequence to an <see cref="IEnumerable{T}" />, 
        /// and flattens the resulting sequences into one sequence. The 
        /// index of each source element is used in the projected form of 
        /// that element.
        /// </summary>

        public static IEnumerable<TResult> SelectMany<TSource, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, int, IEnumerable<TResult>> selector)
        {
            if (selector == null) throw new ArgumentNullException("selector");

            return source.SelectMany(selector, (item, subitem) => subitem);
        }

        /// <summary>
        /// Projects each element of a sequence to an <see cref="IEnumerable{T}" />, 
        /// flattens the resulting sequences into one sequence, and invokes 
        /// a result selector function on each element therein.
        /// </summary>

        public static IEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, IEnumerable<TCollection>> collectionSelector,
            Func<TSource, TCollection, TResult> resultSelector)
        {
            if (collectionSelector == null) throw new ArgumentNullException("collectionSelector");

            return source.SelectMany((item, i) => collectionSelector(item), resultSelector);
        }

        /// <summary>
        /// Projects each element of a sequence to an <see cref="IEnumerable{T}" />, 
        /// flattens the resulting sequences into one sequence, and invokes 
        /// a result selector function on each element therein. The index of 
        /// each source element is used in the intermediate projected form 
        /// of that element.
        /// </summary>

        public static IEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, int, IEnumerable<TCollection>> collectionSelector,
            Func<TSource, TCollection, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (collectionSelector == null) throw new ArgumentNullException("collectionSelector");
            if (resultSelector == null) throw new ArgumentNullException("resultSelector");

            return SelectManyYield(source, collectionSelector, resultSelector);
        }

        private static IEnumerable<TResult> SelectManyYield<TSource, TCollection, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, int, IEnumerable<TCollection>> collectionSelector,
            Func<TSource, TCollection, TResult> resultSelector)
        {
            var i = 0;
            foreach (var item in source)
                foreach (var subitem in collectionSelector(item, i++))
                    yield return resultSelector(item, subitem);
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

    internal class Set<TElement>
    {
        private int[] buckets;
        private Set<TElement>.Slot[] slots;
        private int count;
        private int freeList;
        private IEqualityComparer<TElement> comparer;

        public Set()
            : this((IEqualityComparer<TElement>)null)
        {
        }

        public Set(IEqualityComparer<TElement> comparer)
        {
            if (comparer == null)
                comparer = (IEqualityComparer<TElement>)EqualityComparer<TElement>.Default;
            this.comparer = comparer;
            this.buckets = new int[7];
            this.slots = new Set<TElement>.Slot[7];
            this.freeList = -1;
        }

        public bool Add(TElement value)
        {
            return !this.Find(value, true);
        }

        public bool Contains(TElement value)
        {
            return this.Find(value, false);
        }

        public bool Remove(TElement value)
        {
            int num = this.comparer.GetHashCode(value) & int.MaxValue;
            int index1 = num % this.buckets.Length;
            int index2 = -1;
            for (int index3 = this.buckets[index1] - 1; index3 >= 0; index3 = this.slots[index3].next)
            {
                if (this.slots[index3].hashCode == num && this.comparer.Equals(this.slots[index3].value, value))
                {
                    if (index2 < 0)
                        this.buckets[index1] = this.slots[index3].next + 1;
                    else
                        this.slots[index2].next = this.slots[index3].next;
                    this.slots[index3].hashCode = -1;
                    this.slots[index3].value = default(TElement);
                    this.slots[index3].next = this.freeList;
                    this.freeList = index3;
                    return true;
                }
                index2 = index3;
            }
            return false;
        }

        private bool Find(TElement value, bool add)
        {
            int num = this.comparer.GetHashCode(value) & int.MaxValue;
            for (int index = this.buckets[num % this.buckets.Length] - 1; index >= 0; index = this.slots[index].next)
            {
                if (this.slots[index].hashCode == num && this.comparer.Equals(this.slots[index].value, value))
                    return true;
            }
            if (add)
            {
                int index1;
                if (this.freeList >= 0)
                {
                    index1 = this.freeList;
                    this.freeList = this.slots[index1].next;
                }
                else
                {
                    if (this.count == this.slots.Length)
                        this.Resize();
                    index1 = this.count;
                    ++this.count;
                }
                int index2 = num % this.buckets.Length;
                this.slots[index1].hashCode = num;
                this.slots[index1].value = value;
                this.slots[index1].next = this.buckets[index2] - 1;
                this.buckets[index2] = index1 + 1;
            }
            return false;
        }

        private void Resize()
        {
            int length = checked(this.count * 2 + 1);
            int[] numArray = new int[length];
            Set<TElement>.Slot[] slotArray = new Set<TElement>.Slot[length];
            Array.Copy((Array)this.slots, 0, (Array)slotArray, 0, this.count);
            for (int index1 = 0; index1 < this.count; ++index1)
            {
                int index2 = slotArray[index1].hashCode % length;
                slotArray[index1].next = numArray[index2] - 1;
                numArray[index2] = index1 + 1;
            }
            this.buckets = numArray;
            this.slots = slotArray;
        }

        internal struct Slot
        {
            internal int hashCode;
            internal TElement value;
            internal int next;
        }
    }
}
