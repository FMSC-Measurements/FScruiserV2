using System.Linq;
using System.Collections.Generic;

namespace FSCruiser.Core
{
    public static class DictionaryExtentions
    {
        public static void RemoveByValue<K, V>(this Dictionary<K, V> dict, V value)
        {
            foreach (var item in dict.Where(kvp => kvp.Value.Equals(value)).ToList())
            {
                dict.Remove(item.Key);
            }
        }
    }
}