using System.Linq;
using System.Collections.Generic;

namespace FSCruiser.Core
{
    public static class DictionaryExtentions
    {
        public static void RemoveByValue<TKey, TValue>(this IDictionary<TKey, TValue> dict, TValue value)
        {
            foreach (var item in ((IEnumerable<KeyValuePair<TKey, TValue>>)dict).Where(kvp => kvp.Value.Equals(value)).ToArray())
            {
                dict.Remove(item.Key);
            }
        }
    }
}